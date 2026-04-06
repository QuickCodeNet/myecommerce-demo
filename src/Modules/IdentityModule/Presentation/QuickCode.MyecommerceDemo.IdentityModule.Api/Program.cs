using System;
using Dapper;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Filters;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.Common.Nswag.Extensions;
using QuickCode.MyecommerceDemo.Common.Extensions;
using QuickCode.MyecommerceDemo.IdentityModule.Api.Extension;
using QuickCode.MyecommerceDemo.IdentityModule.Persistence.Contexts;
using QuickCode.MyecommerceDemo.Common.Mappers;
using Serilog;
using QuickCode.MyecommerceDemo.Common.Auditing;
using QuickCode.MyecommerceDemo.Common.Middleware;
using QuickCode.MyecommerceDemo.Common.ExceptionHandling;
using QuickCode.MyecommerceDemo.IdentityModule.Persistence.Seed;

DefaultTypeMap.MatchNamesWithUnderscores = true;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.UpdateConfigurationFromEnv();

var runMigration = Environment.GetEnvironmentVariable("RUN_MIGRATION");
var useHealthCheck = builder.Configuration.GetSection("AppSettings:UseHealthCheck").Get<bool>();
var databaseType = builder.Configuration.GetSection("AppSettings:DatabaseType").Get<string>();
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

builder.Services.AddLogger(builder.Configuration);
Log.Information($"Started({environmentName}) {builder.Configuration["Logging:ApiName"]} Started.");

builder.Services.AddQuickCodeMediator<Program>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
    options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
}).AddJwtBearer(options => {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = Environment.GetEnvironmentVariable("QUICKCODE_JWT_SECRET_KEY"); 
        var securityKey = Encoding.UTF8.GetBytes(secretKey!);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(securityKey)
        };
    })
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.Cookie.Name = "QuickCodeCookieName";
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
        options.AccessDeniedPath = "/api/auth/access-denied";
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new ToKebabParameterTransformer(typeof(Program))));
    options.Filters.Add(typeof(ApiLogFilterAttribute));
    options.Filters.Add(new ProducesAttribute("application/json"));
}).AddJsonOptions(jsonOptions => { jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();
builder.Services.AddQuickCodeIdentityDbContext<AppDbContext>(builder.Configuration);
builder.Services.AddIdentityCore<ApiUser>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>()
    .AddApiEndpoints();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddResponseCompression();
builder.Services.Configure<GzipCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });
builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddBankingGradeAuditing(builder.Configuration);
builder.Services.AddAuditDbContext(builder.Configuration);

builder.Services.AddQuickCodeDbContext<ReadDbContext, WriteDbContext>(builder.Configuration);
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddQuickCodeRepositories(typeof(WriteDbContext));
builder.Services.AddQuickCodeSwaggerGen(builder.Configuration);
builder.Services.AddNswagServiceClient(builder.Configuration, typeof(Program));
builder.Services.AddServiceClient(builder.Configuration, typeof(Program));
builder.Services.AddEndpointsApiExplorer();

DapperTypeMapper.ConfigureTypeMappings();

var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseResponseCompression();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGroup("/api/auth").WithTags("Authentications").MapQuickCodeIdentityApi<ApiUser>();

app.MapControllers();

if (useHealthCheck && databaseType != "inMemory")
{
    app.UseHealthChecks("/hc", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
}

app.UseExceptionHandler();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseSecurityHeaders();
app.UseRateLimiting();
app.UseInputValidation();
app.UseSecurityLogging();
app.UseSecurityAudit();
app.UsePasswordPolicy();
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
    if ((runMigration == null || runMigration!.Equals("yes", StringComparison.CurrentCultureIgnoreCase)) &&
        databaseType != "inMemory")
    {
        await dbContext.Database.MigrateAsync();
        
        var seedRepo = new SeedRepository(dbContext, scope.ServiceProvider);
        await DatabaseSeeder.SeedAsync(dbContext, seedRepo);
    }
}

await app.RunAsync();