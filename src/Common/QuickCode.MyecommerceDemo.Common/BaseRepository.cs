using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using QuickCode.MyecommerceDemo.Common.Extensions;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.Common;

public abstract class BaseRepository(ILogger logger, string repoName)
{
    protected async Task<RepoResponse<T>> ExecuteWithExceptionHandling<T>(string operation, Func<Task<RepoResponse<T>>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception ex)
        {
            return logger.LogExceptionAndCreateResponse<T>(ex, repoName, operation);
        }
    }

    protected IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;
        return query.Skip(skip).Take(pageSize);
    }
    
    protected RepoResponse<T> CreateNotFoundResponse<T>(string message)
    {
        return new RepoResponse<T>
        {
            Code = 404,
            Message = message
        };
    }
    
    protected RepoResponse<List<T>> BuildListResponse<T>(IEnumerable<T> values, string notFoundMessage = "Not found")
    {
        return values?.Any() == true ? new RepoResponse<List<T>> { Value = values.ToList() } : CreateNotFoundResponse<List<T>>(notFoundMessage);
    }
    
    protected RepoResponse<T> BuildResponse<T>(T value, string notFoundMessage = "Not found")
    {
        return value is not null ? new RepoResponse<T> { Value = value } : CreateNotFoundResponse<T>(notFoundMessage);
    }

    protected RepoResponse<bool> BuildBoolResponse(bool exists, string notFoundMessage = "Not found")
    {
        return exists ? new RepoResponse<bool> { Value = exists } : CreateNotFoundResponse<bool>(notFoundMessage);
    }
    
    protected async Task<DbConnection> GetOpenConnectionAsync(DbContext dbContext)
    {
        var connection = dbContext.Database.GetDbConnection();
        
        if (string.IsNullOrEmpty(connection.ConnectionString))
        {
            if (dbContext is Microsoft.EntityFrameworkCore.Infrastructure.IInfrastructure<IServiceProvider> infrastructure)
            {
                var serviceProvider = infrastructure.Instance;
                var options = serviceProvider.GetService<Microsoft.EntityFrameworkCore.Infrastructure.IDbContextOptions>();
                if (options != null)
                {
                    var extension = options.Extensions.OfType<Microsoft.EntityFrameworkCore.Infrastructure.RelationalOptionsExtension>().FirstOrDefault();
                    if (extension?.ConnectionString != null)
                    {
                        connection.ConnectionString = extension.ConnectionString;
                    }
                }
            }
            
            if (string.IsNullOrEmpty(connection.ConnectionString))
            {
                throw new InvalidOperationException("Connection string is not initialized on DbContext. Ensure DbContext is properly configured with connection string.");
            }
        }
        
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        return connection;
    }
}