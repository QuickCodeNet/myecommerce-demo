using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace QuickCode.MyecommerceDemo.Common.Auditing;

public static class AuditingExtensions
{
    public static IServiceCollection AddBankingGradeAuditing(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AuditingOptions>(
            configuration.GetSection(AuditingOptions.SectionName));
        
        services.AddSingleton<IAuditLogWriter, BackgroundAuditLogWriter>();
        services.AddHostedService(provider => 
            (BackgroundAuditLogWriter)provider.GetRequiredService<IAuditLogWriter>());
        
        services.AddScoped<AuditLogInterceptor>();
        
        return services;
    }
    
    public static IServiceCollection AddAuditDbContext(
        this IServiceCollection services, 
        IConfiguration configuration,
        string connectionStringName = "WriteConnectionString")
    {
        var databaseType = configuration.GetSection("AppSettings:DatabaseType").Get<string>();
        var connectionString = configuration.GetConnectionString(connectionStringName);

        switch (databaseType?.ToLowerInvariant())
        {
            case "mssql":
                services.AddDbContext<AuditDbContext>(options =>
                    options.UseSqlServer(connectionString));
                break;
                
            case "postgresql":
                services.AddDbContext<AuditDbContext>(options =>
                    options.UseNpgsql(connectionString));
                break;
                
            case "mysql":
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
                services.AddDbContext<AuditDbContext>(options =>
                    options.UseMySql(connectionString, serverVersion, 
                        o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore)));
                break;
                
            case "inmemory":
                services.AddDbContext<AuditDbContext>(options =>
                    options.UseInMemoryDatabase("AuditDb"));
                break;
                
            default:
                throw new InvalidOperationException($"Unsupported database type: {databaseType}");
        }

        return services;
    }
    
    public static DbContextOptionsBuilder AddAuditLogging(
        this DbContextOptionsBuilder optionsBuilder,
        IServiceProvider serviceProvider)
    {
        var interceptor = serviceProvider.GetRequiredService<AuditLogInterceptor>();
        optionsBuilder.AddInterceptors(interceptor);
        return optionsBuilder;
    }
}

