using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using QuickCode.MyecommerceDemo.Common.Auditing;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Swashbuckle.AspNetCore.SwaggerGen;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.Common;

public static class ServiceCollectionExtensions
{
    private const string ElasticSearchUriKey = "Logging:ElasticConfiguration:Uri";
    private const string WriteConnectionStringName = "WriteConnectionString";
    private const string ReadConnectionStringName = "ReadConnectionString";

    public static IServiceCollection AddQuickCodeSwaggerGen(this IServiceCollection services,IConfiguration configuration, Action<SwaggerGenOptions>? configureOptions=null, string tokenType = "ApiKey")
    {
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName);
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = $"{configuration["Swagger:Title"]} ({Assembly.GetExecutingAssembly().GetName().Version})",
                TermsOfService = new Uri(configuration["Swagger:TermsOfService"]!),
                Contact = new OpenApiContact
                {
                    Name = "Üzeyir Apaydın",
                    Url = new Uri(configuration["Swagger:Contact"]!)
                },
                License = new OpenApiLicense
                {
                    Name = "QuickCode.Net License",
                    Url = new Uri(configuration["Swagger:License"]!)
                }
            });

            // options.CustomOperationIds(e =>
            //     {
            //         var relativePath = e.RelativePath!.Replace("/", "-")
            //             .Replace("{", "-")
            //             .Replace("}", "-");
            //         var operationId = $"{relativePath}-{e.HttpMethod!.ToLowerInvariant()}".Replace("--", "-");
            //         return operationId;
            //     }
            // );
            
            options.CustomOperationIds(e =>
            {
                try
                {
                    var controllerName = e.ActionDescriptor.RouteValues["controller"];
                    var actionName = e.ActionDescriptor.RouteValues["action"];
                    var operationId = $"{controllerName}{actionName}";
                    return operationId;
                }
                catch(Exception ex)
                {
                    var relativePath = e.RelativePath!.Replace("/", "-")
                        .Replace("{", "-")
                        .Replace("}", "-");
                    var operationId = $"{relativePath}-{e.HttpMethod!.ToLowerInvariant()}".Replace("--", "-");
                    return operationId;
                }
            });
            
            switch (tokenType)
            {
                case "Bearer":
                    options.AddSecurityDefinition(tokenType, new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = tokenType,
                        Scheme = tokenType
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = tokenType
                                },
                            },
                            new List<string>() { }
                        }
                    });
                    break;
                case "ApiKey":
                    options.AddSecurityDefinition(tokenType, new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid Api Key",
                        Name = "X-Api-Key",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = $"{tokenType}Schema"
                    });
                    
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = tokenType
                                },
                            },
                            new List<string>() { }
                        }
                    });
                    break;
            }

            configureOptions?.Invoke(options);

            options.SchemaFilter<EnumSchemaFilter>();
        });

        return services;
    }


    private static void AddMsSqlDbContext<T>(this IServiceCollection services, IConfiguration configuration,
        bool useHealthCheck, string connectionStringName) where T : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

        services.AddDbContext<T>((sp,options) =>
        {
            options.UseSqlServer(connectionString)
                .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>())
                .AddAuditLogging(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        if (useHealthCheck)
        {
            var elasticConnectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING") ??
                                          configuration[ElasticSearchUriKey]!;
            services
                .AddHealthChecks()
                .AddSqlServer(connectionString!)
                .AddElasticsearch(elasticConnectionString);
        }
    }
    
    private static void AddMySqlDbContext<T>(this IServiceCollection services, IConfiguration configuration,
        bool useHealthCheck, string connectionStringName) where T : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
        services.AddDbContext<T>((sp, options) =>
        {
            options.UseMySql(connectionString, serverVersion, o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore))
                .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>())
                .AddAuditLogging(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        if (useHealthCheck)
        {
            var elasticConnectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING") ??
                                          configuration[ElasticSearchUriKey]!;
            
            services
                .AddHealthChecks()
                .AddMySql(connectionString!)
                .AddElasticsearch(elasticConnectionString);
        }
    }
    
    private static void AddInMemoryDbContext<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        var databaseName = configuration.GetSection("AppSettings:InMemoryDbName").Get<string>();
        services.AddDbContext<T>((sp, options) => { options.UseInMemoryDatabase(databaseName!); });
    }

    public static void AddQuickCodeDbContext<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        var useHealthCheck = configuration.GetSection("AppSettings:UseHealthCheck").Get<bool>();
        var databaseType = configuration.GetSection("AppSettings:DatabaseType").Get<string>();
        services.AddSingleton<SoftDeleteInterceptor>();

        switch (databaseType!)
        {
            case "mssql":
                services.AddMsSqlDbContext<T>(configuration, useHealthCheck, WriteConnectionStringName);
                break;
            case "postgresql":
                services.AddPostgresSqlServerDbContext<T>(configuration, useHealthCheck, WriteConnectionStringName);
                break;
            case "mysql":
                services.AddMySqlDbContext<T>(configuration, useHealthCheck, WriteConnectionStringName);
                break;
            case "inMemory":
                services.AddInMemoryDbContext<T>(configuration);
                break;
        }
    }

    public static void AddQuickCodeDbContext<TReadContext, TWriteContext>(this IServiceCollection services, IConfiguration configuration)
        where TReadContext : DbContext
        where TWriteContext : DbContext
    {
        var useHealthCheck = configuration.GetSection("AppSettings:UseHealthCheck").Get<bool>();
        var databaseType = configuration.GetSection("AppSettings:DatabaseType").Get<string>();
        
        services.AddSingleton<SoftDeleteInterceptor>();
        
        switch (databaseType!)
        {
            case "mssql":
                services.AddMsSqlDbContext<TWriteContext>(configuration, useHealthCheck, WriteConnectionStringName);
                services.AddMsSqlDbContext<TReadContext>(configuration, false, ReadConnectionStringName);
                break;
            case "postgresql":
                services.AddPostgresSqlServerDbContext<TWriteContext>(configuration, useHealthCheck, WriteConnectionStringName);
                services.AddPostgresSqlServerDbContext<TReadContext>(configuration, false, ReadConnectionStringName);
                break;
            case "mysql":
                services.AddMySqlDbContext<TWriteContext>(configuration, useHealthCheck, WriteConnectionStringName);
                services.AddMySqlDbContext<TReadContext>(configuration, false, ReadConnectionStringName);
                break;
            case "inMemory":
                services.AddInMemoryDbContext<TWriteContext>(configuration);
                services.AddInMemoryDbContext<TReadContext>(configuration);
                break;
        }
    }
    
    public static void AddQuickCodeIdentityDbContext<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        var useHealthCheck = false;
        var databaseType = configuration.GetSection("AppSettings:DatabaseType").Get<string>();

        switch (databaseType!)
        {
            case "mssql":
                services.AddMsSqlDbContext<T>(configuration, useHealthCheck, WriteConnectionStringName);
                break;
            case "postgresql":
                services.AddPostgresSqlServerDbContext<T>(configuration, useHealthCheck, WriteConnectionStringName);
                break;
            case "mysql":
                services.AddMySqlDbContext<T>(configuration, useHealthCheck, WriteConnectionStringName);
                break;
            case "inMemory":
                services.AddInMemoryDbContext<T>(configuration);
                break;
        }
    }

    private static void AddPostgresSqlServerDbContext<T>(this IServiceCollection services, IConfiguration configuration,
        bool useHealthCheck, string connectionStringName) where T : DbContext
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);

        services.AddDbContext<T>((sp,options) =>
        {
            options.UseNpgsql(connectionString)
                .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>())
                .AddAuditLogging(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        
        if (useHealthCheck)
        {
            var elasticConnectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING") ??
                                          configuration[ElasticSearchUriKey]!;
            services
                .AddHealthChecks()
                .AddNpgSql(connectionString!)
                .AddElasticsearch(elasticConnectionString);
        }
    }
    
    public static void AddLogger(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            var excludingFilters = new Dictionary<string, List<string>>()
            {
                { "RequestPath", ["/hc"] }
            };

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .Filter.ByExcluding(e =>
                    excludingFilters.Keys.Any(filterKey =>
                        e.Properties.ContainsKey(filterKey) &&
                        excludingFilters[filterKey].Contains(((ScalarValue)e.Properties[filterKey]).Value!.ToString()!)))
                .ReadFrom.Configuration(configuration)
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment!))
                .Enrich.WithProperty("Environment", environment!)
                .CreateLogger();

            services.AddSerilog(logger);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
    {
        var elasticConnectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING") ??
                                      configuration[ElasticSearchUriKey]!;
        
        return new ElasticsearchSinkOptions(new Uri(elasticConnectionString))
        {
            AutoRegisterTemplate = true,
            IndexFormat =
                $"{configuration["Logging:ApiName"]}--{environment?.ToLower()
                    .Replace(".", "-")}--{DateTime.UtcNow:yyyy-MM-dd}"
        };
    }
}