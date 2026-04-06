using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using QuickCode.MyecommerceDemo.Common;
using QuickCode.MyecommerceDemo.Common.Extensions;
using QuickCode.MyecommerceDemo.Gateway.Messaging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Extensions;
using QuickCode.MyecommerceDemo.Common.Nswag.Clients.IdentityModuleApi.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using QuickCode.MyecommerceDemo.Common.Controllers;
using Yarp.ReverseProxy.Transforms;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Nswag;
using QuickCode.MyecommerceDemo.Common.Nswag.Extensions;
using QuickCode.MyecommerceDemo.Gateway.Models;
using QuickCode.MyecommerceDemo.Gateway.Extensions;
using QuickCode.MyecommerceDemo.Gateway.KafkaProducer;
using Serilog;
using QuickCode.MyecommerceDemo.Common.Middleware;
using QuickCode.MyecommerceDemo.Gateway.Middleware;

using InMemoryConfigProvider = QuickCode.MyecommerceDemo.Gateway.Extensions.InMemoryConfigProvider;

var builder = WebApplication.CreateBuilder(args);

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
Log.Information($"Started({environmentName})...");

ConfigureEnvironmentVariables(builder.Configuration);

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

builder.Services.AddSingleton<IKafkaProducerWrapper, KafkaProducerWrapper>();

builder.Services
    .AddReverseProxy()
    .ConfigureHttpClient((context, handler) =>
    {
        handler.AllowAutoRedirect = true;
    })
    .LoadFromMemory()
    .AddTransforms(context =>
    {
        context.RequestTransforms.Add(new RequestHeaderRemoveTransform("Cookie"));
    });

builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddQuickCodeSwaggerGen(builder.Configuration);
builder.Services.AddNswagServiceClient(builder.Configuration, typeof(QuickCodeBaseApiController));
builder.Services.AddCustomHealthChecks(builder.Configuration);
builder.Services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);

var app = builder.Build();

builder.Services.AddLogger(builder.Configuration);
Log.Information($"{builder.Configuration["Logging:ApiName"]} Started.");

ConfigureMiddlewares();

await app.RunAsync();

void ConfigureMiddlewares()
{
    app.UseExceptionHandler("/error");

	if (!app.Environment.IsDevelopment())
	{
    	app.UseHsts();
	}
    
    app.UseGatewaySecurityHeaders(); 
    app.UseRateLimiting();
    app.UseInputValidation();
    app.UseSecurityLogging();
    app.UseSecurityAudit();
    app.UsePasswordPolicy();
    
    app.UseSwagger();
    app.UseSwaggerUI();

    if (app.Configuration.GetValue<bool>("AppSettings:UseHealthCheck"))
    {
        app.UseHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHealthChecksUI(config => { config.UIPath = "/hc-ui"; });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    var corsAllowedUrls = app.Configuration.GetSection("CorsSettings:AllowOrigins").Get<string[]>();
    app.UseCors(x => x
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins(corsAllowedUrls!)
        .SetIsOriginAllowedToAllowWildcardSubdomains());

    var gatewayGroup = app.MapGroup("/api/gateway").WithTags("Gateway");
    
    app.MapGet("/", GetServicesHtml).WithTags("Dashboard");

    app.Map("/error", (HttpContext context) =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var isDev = app.Environment.IsDevelopment();

        return Results.Problem(
            title: "Unexpected error Gateway",
            detail: isDev ? exception?.Message : "Something went wrong.",
            statusCode: 500
        );
    }).ExcludeFromDescription();

    gatewayGroup.MapGet("/reset", () =>
    {
        if (InMemoryConfigProvider.IsClustersUpdatedFromConfig == 1)
        {
            InMemoryConfigProvider.IsClustersUpdatedFromConfig = -1;
        }
    });

    gatewayGroup.MapGet("/config", () => InMemoryConfigProvider.proxyConfig.ReverseProxy.ToJson());

    gatewayGroup.MapGet("/swagger-config", () => InMemoryConfigProvider.swaggerMaps.ToJson());

    InMemoryConfigProvider.app = app;
  
    app.MapReverseProxy(proxyPipeline =>
    {
        proxyPipeline.Use(YarpMiddlewareKafkaManager(app.Services));
        proxyPipeline.Use(YarpMiddlewareApiAuthorization(app.Services));
    });
}

void ConfigureEnvironmentVariables(IConfiguration configuration)
{
    configuration.UpdateConfigurationFromEnv();

    builder.Services.AddAuthorizationBuilder()
        .AddPolicy("QuickCodeGatewayPolicy", policy =>
        {
            policy.RequireAssertion(context =>
            {
                var httpContext = context.Resource as HttpContext;
                if (httpContext == null)
                {
                    return false;
                }

                var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (!string.IsNullOrEmpty(token) && token.IsTokenExpired()) 
                {
                    httpContext.Response.Headers.Append("Token-Expired", "true");
                }

                return true;
            });
        });

    Console.WriteLine($"environmentName : {environmentName}");
}


Func<HttpContext, Func<Task>, Task> YarpMiddlewareKafkaManager(IServiceProvider services)
{
    return async (context, next) =>
    {
        try
        {
            var memoryCache = services.GetRequiredService<IMemoryCache>();
            var kafkaProducer = services.GetRequiredService<IKafkaProducerWrapper>();
            var originalBodyStream = context.Response.Body;
            context.Request.EnableBuffering();

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                await next();
                _ = Task.Run(async () =>
                {
                    await KafkaHelper.SendKafkaMessageIfEventExists(services, memoryCache, kafkaProducer, context, stopwatch);
                });
            }
            catch (Exception ex)
            {
                var kafkaEvent = await KafkaHelper.CheckKafkaEventExists(services, memoryCache, context);
                if (kafkaEvent is not null)
                {
                    _ = Task.Run(async () =>
                    {
                        await KafkaHelper.SendErrorKafkaMessage(kafkaProducer, kafkaEvent.TopicName, context, stopwatch, ex);
                    });
                }

                throw;
            }
            finally
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                stopwatch.Stop();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    };
}

Func<HttpContext, Func<Task>, Task> YarpMiddlewareApiAuthorization(IServiceProvider services)
{
    return async (context, next) =>
    {
        var memoryCache = services.GetRequiredService<IMemoryCache>();
        var configuration = services.GetRequiredService<IConfiguration>();
        var token = ExtractToken(context);
        var cacheKey = $"AuthJwtTokens-{token}";
        
        if (string.IsNullOrEmpty(token))
        {
            await next();
            return;
        }
        
        if (token.IsTokenExpired())
        {
            memoryCache.Remove(cacheKey);
            context.Response.Headers.Append("Token-Expired", "true");
            AppendApiKey(context, configuration);
            EnsureForwardedUserAgent(context);
            await next();
            return;
        }

        if (!await ValidateAndProcessToken(context, services, token, cacheKey))
        {
            return;
        }

        await next();
    };
}



string ExtractToken(HttpContext context)
{
    return context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last() ?? string.Empty;
}

bool HandleTokenExpiration(string token, IMemoryCache memoryCache, string cacheKey)
{
    if (!string.IsNullOrEmpty(token) && token.IsTokenExpired())
    {
        memoryCache.Remove(cacheKey);
        return true;
    }
    
    return false;
}

void HandleEmptyToken(HttpContext context, IMemoryCache memoryCache, string cacheKey)
{
    if (!context.Request.Path.Value!.StartsWith("/api/auth/logout"))
    {
        memoryCache.Remove(cacheKey);
    }
}

async Task<bool> ValidateAndProcessToken(HttpContext context, IServiceProvider services, string token, string cacheKey)
{
    var memoryCache = services.GetRequiredService<IMemoryCache>();
    var configuration = services.GetRequiredService<IConfiguration>();

    if (token.IsTokenExpired())
    {
        context.Response.Headers.Append("Token-Expired", "true");
    }

    var isValidToken = await ValidateToken(services, token, cacheKey, memoryCache);
    if (!isValidToken)
    {
        await HandleInvalidToken(context);
        return false;
    }

    var jwtClaims = token.ParseJwtPayload();
    var permissionGroupName = jwtClaims["PermissionGroupName"];

    if (!await IsMethodValid(context, services, permissionGroupName, memoryCache))
    {
        await HandleInvalidMethod(context);
        return false;
    }

    AppendApiKey(context, configuration);
    EnsureForwardedUserAgent(context);

    return true;
}

async Task<bool> ValidateToken(IServiceProvider services, string token, string cacheKey, IMemoryCache memoryCache)
{
    return !token.IsTokenExpired() && await memoryCache.GetOrAddAsync<bool>(cacheKey,
        async parameters => await KafkaHelper.GetTokenIsValid(services, token),
        TimeSpan.FromSeconds(token.GetTokenExpirationTime()));
}

async Task HandleInvalidToken(HttpContext context)
{
    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    await context.Response.WriteAsync("Token is invalid");
}

async Task<bool> IsMethodValid(HttpContext context, IServiceProvider services, string permissionGroupName, IMemoryCache memoryCache)
{
    var groupMethods = await GetGroupMethods(services, memoryCache);
    var path = context.Request.Path.Value!;
    var validPaths = groupMethods
        .Where(i => i.PermissionGroupName == permissionGroupName && i.HttpMethod.ToString().ToLowerInvariant()
            .Equals(context.Request.Method.ToLowerInvariant()))
        .Select(i => i.Path)
        .ToList();

    return path.IsRouteMatch(validPaths) || path.StartsWith("/api/auth/login");
}


async Task<List<GroupHttpMethodPath>> GetGroupMethods(IServiceProvider services, IMemoryCache memoryCache)
{
    var configuration = services.GetRequiredService<IConfiguration>();
    var ApiMethodAccessGrantsClient = services.GetRequiredService<IApiMethodAccessGrantsClient>();
    var apiMethodDefinitionsClient = services.GetRequiredService<IApiMethodDefinitionsClient>();

    return await memoryCache.GetOrAddAsync<List<GroupHttpMethodPath>>("GroupMethods",
        async parameters => await GetAllGroupMethods(configuration, ApiMethodAccessGrantsClient, apiMethodDefinitionsClient),
        TimeSpan.FromMinutes(1));
}

async Task HandleInvalidMethod(HttpContext context)
{
    context.Response.StatusCode = StatusCodes.Status403Forbidden;
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { message = "Method Is Not Valid" }));
}

void AppendApiKey(HttpContext context, IConfiguration configuration)
{
    var moduleName = context.GetEndpoint()!.DisplayName!.KebabCaseToPascal("");
    var apiKeyConfigValue = $"QuickcodeApiKeys:{moduleName}ApiKey";
    var configApiKey = configuration.GetValue<string>(apiKeyConfigValue);

    if (configApiKey != null)
    {
        context.Request.Headers.Append("X-Api-Key", configApiKey);
    }
}

void EnsureForwardedUserAgent(HttpContext context)
{
    if (context.Request.Headers.TryGetValue("X-Forwarded-User-Agent", out var forwardedUserAgent)
        && !StringValues.IsNullOrEmpty(forwardedUserAgent))
    {
        return;
    }

    if (context.Request.Headers.TryGetValue("User-Agent", out var userAgent) && !StringValues.IsNullOrEmpty(userAgent))
    {
        context.Request.Headers["X-Forwarded-User-Agent"] = userAgent;
    }
}


IResult GetServicesHtml(HttpContext context)
{
    var portalUrlConfig = app.Configuration.GetSection("AppSettings:PortalUrl").Get<string>();
    var elasticUrl = app.Configuration.GetSection("AppSettings:ElasticUrl").Get<string>();
    var kafdropUrl = app.Configuration.GetSection("AppSettings:KafdropUrl").Get<string>();
    var eventListenerUrlConfig =  app.Configuration.GetSection("AppSettings:EventListenerUrl").Get<string>();
    const string swaggerJson = "/v1/swagger.json";
    const string swaggerHtml = "/index.html";
    
    const string quickcodeBaseUrl = ".quickcode.net";
    const string cloudRunBaseUrl = "-821209474183.europe-west1.run.app";

    var request = context.Request;


    var scheme = request.Scheme;  
    var host = request.Host.Value; 
    var path = request.Path;  
    var query = request.QueryString; 

    var displayUrl = $"{scheme}://{host}{path}{query}";
    
    var isRewriteUrl = displayUrl.Contains(quickcodeBaseUrl);
    var portalUrl = isRewriteUrl ? portalUrlConfig!.Replace(cloudRunBaseUrl, quickcodeBaseUrl) : portalUrlConfig!;
    var eventListenerUrl = isRewriteUrl
        ? eventListenerUrlConfig!.Replace(cloudRunBaseUrl, quickcodeBaseUrl)
		: eventListenerUrlConfig!;
    

    var destinations = InMemoryConfigProvider.swaggerMaps.Select(c => new
    {
        ClusterId = c.Key,
        Address = isRewriteUrl
            ? c.Value.Endpoint.Replace(swaggerJson, swaggerHtml).Replace(cloudRunBaseUrl, quickcodeBaseUrl)
            : c.Value.Endpoint.Replace(swaggerJson, swaggerHtml)
    }).ToList();
    
    destinations.Add(new { ClusterId = "Event Listener Service", Address = $"{eventListenerUrl}/swagger/index.html" }!);

    var tabsComboBoxHtml = destinations.Select((value, index) =>
        $"<li><a id=\"{value.ClusterId.ToLower().Replace(" ", "_")}\" data-toggle=\"tab\"  class=\"dropdown-item\" href=\"{value.Address}\">{value.ClusterId.KebabCaseToPascal()}</a></li>");

    var lastUpdate = DateTime.UtcNow - InMemoryConfigProvider.LastUpdateDateTime;
    var lastUpdateValue = $"{lastUpdate.TotalSeconds:0}s ago";
    if (lastUpdate.Minutes > 0)
    {
        lastUpdateValue = $"{lastUpdate.TotalMinutes:0}m {(lastUpdate.TotalSeconds % 60):0}s ago";
    }

    if (lastUpdate.Hours > 0)
    {
        lastUpdateValue = $"{lastUpdate.TotalHours:0}h {(lastUpdate.TotalMinutes % 60):0}m ago";
    }

    var projectName = typeof(ReverseProxyConfigModel).Namespace!.Split(".")[1];
    var fileContent = File.ReadAllText("Dashboard/Dashboard.html");
    var tabsContent = string.Join("<li><hr class=\"dropdown-divider\"></li>", tabsComboBoxHtml.ToArray());

    var githubUrl = $"https://github.com/QuickCodeNet/{projectName.ToLower()}";
    var isHttpsText = "<meta http-equiv=\"Content-Security-Policy\" content=\"upgrade-insecure-requests\">";
    
    if (environmentName == "Local")
    {
        isHttpsText = "";
    }
    
    isHttpsText = "";
    fileContent = fileContent.Replace("<!--|@TABS@|-->", string.Join("", tabsContent));
    fileContent = fileContent.Replace("<!--|@TABS_COUNT@|-->", destinations.Count().ToString());
    fileContent = fileContent.Replace("<!--|@LAST_UPDATE@|-->", lastUpdateValue);
    fileContent = fileContent.Replace("<!--|@ENVIRONMENT@|-->", environmentName);
    fileContent = fileContent.Replace("<!--|@PROJECT_NAME@|-->", projectName);
    fileContent = fileContent.Replace("<!--|@PROJECT_NAME_LOWER@|-->", projectName.ToLower());
    fileContent = fileContent.Replace("<!--|@PORTAL_URL@|-->", portalUrl);
    fileContent = fileContent.Replace("<!--|@ELASTIC_URL@|-->", elasticUrl);
    fileContent = fileContent.Replace("<!--|@GITHUB_URL@|-->", githubUrl);
    fileContent = fileContent.Replace("<!--|@KAFDROP_URL@|-->", kafdropUrl);
    fileContent = fileContent.Replace("<!--|@EVENT_LISTENER_URL@|-->", eventListenerUrl);
    fileContent = fileContent.Replace("<!--|@VERSION@|-->", $"{Assembly.GetExecutingAssembly().GetName().Version}");
    fileContent = fileContent.Replace("<!--|@IS_HTTPS@|-->", isHttpsText);
    return Results.Extensions.Html(@$"{fileContent}");
}


async Task<List<GroupHttpMethodPath>> GetAllGroupMethods(IConfiguration configuration, IApiMethodAccessGrantsClient ApiMethodAccessGrantsClient, IApiMethodDefinitionsClient apiMethodDefinitionsClient)
{
    
    var apiKeyConfigValue = $"QuickcodeApiKeys:IdentityModuleApiKey";
    var configApiKey = configuration.GetValue<string>(apiKeyConfigValue);
    SetApiKeyToClients(ApiMethodAccessGrantsClient, apiMethodDefinitionsClient, configApiKey!);
    var authorizationsGroups = await ApiMethodAccessGrantsClient.ApiMethodAccessGrantsListAsync();
    var authorizations = await apiMethodDefinitionsClient.ApiMethodDefinitionsListAsync();
    
    SetApiKeyToClients(ApiMethodAccessGrantsClient, apiMethodDefinitionsClient, "");
    var authorizationsGroupsActive = authorizationsGroups.Where(i => i.IsActive);
    
    var allMethods = from authGroup in authorizationsGroupsActive
        join a in authorizations on authGroup.ApiMethodDefinitionKey equals a.Key
        select new GroupHttpMethodPath()
        {
            PermissionGroupName = authGroup.PermissionGroupName, HttpMethod = a.HttpMethod, Path = a.UrlPath
        };

    return allMethods.ToList();
}

void SetApiKeyToClients(IApiMethodAccessGrantsClient ApiMethodAccessGrantsClient, IApiMethodDefinitionsClient apiMethodDefinitionsClient, string configIdentityApiKey)
{
    (ApiMethodAccessGrantsClient as ClientBase)!.SetApiKey(configIdentityApiKey);
    (apiMethodDefinitionsClient as ClientBase)!.SetApiKey(configIdentityApiKey);
}