using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace QuickCode.MyecommerceDemo.Common.Auditing;

public sealed class AuditLogInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuditLogWriter _auditLogWriter;
    private readonly ILogger<AuditLogInterceptor> _logger;
    private readonly AuditingOptions _options;

    public AuditLogInterceptor(
        IHttpContextAccessor httpContextAccessor,
        IAuditLogWriter auditLogWriter,
        ILogger<AuditLogInterceptor> logger,
        IOptions<AuditingOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _auditLogWriter = auditLogWriter;
        _logger = logger;
        _options = options.Value;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var auditEntries = CreateAuditEntries(eventData.Context);

        var interceptResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        
        if (auditEntries.Any())
        {
            var auditLogs = auditEntries.Select(MapToAuditLog).ToList();
            
            _ = Task.Run(async () =>
            {
                try
                {
                    await _auditLogWriter.QueueAuditLogsAsync(auditLogs, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to queue audit logs");
                }
            }, cancellationToken);
        }

        return interceptResult;
    }

    private List<AuditEntry> CreateAuditEntries(DbContext context)
    {
        var auditEntries = new List<AuditEntry>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is not IAuditableEntity ||
                entry.State == EntityState.Detached ||
                entry.State == EntityState.Unchanged)
            {
                continue;
            }
            
            if (entry.Entity is AuditLog)
            {
                continue;
            }

            var auditEntry = new AuditEntry
            {
                EntityName = entry.Entity.GetType().Name,
                Action = entry.State.ToString().ToUpperInvariant(),
                Timestamp = DateTime.UtcNow,
                UserId = GetCurrentUserId(),
                UserName = GetCurrentUserName(),
                UserGroup = GetCurrentUserGroup(),
                IpAddress = GetIpAddress(),
                UserAgent = GetUserAgent(),
                CorrelationId = GetCorrelationId(),
                EntityId = GetPrimaryKeyValue(entry)
            };
            
            switch (entry.State)
            {
                case EntityState.Added:
                    CaptureAddedValues(entry, auditEntry);
                    break;

                case EntityState.Modified:
                    CaptureModifiedValues(entry, auditEntry);
                    break;

                case EntityState.Deleted:
                    CaptureDeletedValues(entry, auditEntry);
                    break;
            }

            auditEntries.Add(auditEntry);
        }

        return auditEntries;
    }

    private void CaptureAddedValues(EntityEntry entry, AuditEntry auditEntry)
    {
        if (_options.CaptureFullEntityOnInsert)
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                    continue;

                auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
            }
        }
        else
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                    continue;

                var currentValue = property.CurrentValue;
                if (currentValue != null)
                {
                    auditEntry.NewValues[property.Metadata.Name] = currentValue;
                }
            }
        }
    }

    private void CaptureModifiedValues(EntityEntry entry, AuditEntry auditEntry)
    {
        auditEntry.ChangeSummaryData = new Dictionary<string, ChangeSummaryItem>();
        var hasChanges = false;
        
        if (_options.CaptureFullEntityOnUpdate)
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                    continue;

                var oldValue = property.OriginalValue;
                var newValue = property.CurrentValue;

                auditEntry.OldValues[property.Metadata.Name] = oldValue;
                auditEntry.NewValues[property.Metadata.Name] = newValue;

                if (property.IsModified)
                {
                    var isChanged = !Equals(oldValue, newValue);
                    
                    if (isChanged)
                    {
                        auditEntry.ChangedColumns.Add(property.Metadata.Name);

                        auditEntry.ChangeSummaryData[property.Metadata.Name] = new ChangeSummaryItem
                        {
                            Old = oldValue,
                            New = newValue
                        };
                        hasChanges = true;
                    }
                }
            }
        }
        else
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                    continue;

                if (!property.IsModified)
                    continue;

                var oldValue = property.OriginalValue;
                var newValue = property.CurrentValue;
                var isChanged = !Equals(oldValue, newValue);
                
                if (isChanged)
                {
                    auditEntry.ChangedColumns.Add(property.Metadata.Name);
                    auditEntry.OldValues[property.Metadata.Name] = oldValue;
                    auditEntry.NewValues[property.Metadata.Name] = newValue;
                    
                    auditEntry.ChangeSummaryData[property.Metadata.Name] = new ChangeSummaryItem
                    {
                        Old = oldValue,
                        New = newValue
                    };
                    hasChanges = true;
                }
            }
        }
        
        auditEntry.IsChanged = hasChanges;
    }

    private void CaptureDeletedValues(EntityEntry entry, AuditEntry auditEntry)
    {
        if (_options.CaptureFullEntityOnDelete)
        {
            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                    continue;

                auditEntry.OldValues[property.Metadata.Name] = property.OriginalValue;
            }
        }
    }

    private AuditLog MapToAuditLog(AuditEntry entry)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        return new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityName = entry.EntityName,
            EntityId = entry.EntityId,
            Action = entry.Action,
            UserId = entry.UserId,
            UserName = entry.UserName,
            UserGroup = entry.UserGroup,
            Timestamp = entry.Timestamp,
            IpAddress = entry.IpAddress,
            UserAgent = entry.UserAgent,
            CorrelationId = entry.CorrelationId,
            IsSuccess = entry.IsSuccess,
            ErrorMessage = entry.ErrorMessage,
            OldValues = entry.OldValues.Any() ? JsonSerializer.Serialize(entry.OldValues, jsonOptions) : null,
            NewValues = entry.NewValues.Any() ? JsonSerializer.Serialize(entry.NewValues, jsonOptions) : null,
            ChangedColumns = entry.ChangedColumns.Any() ? string.Join(",", entry.ChangedColumns) : null,
            IsChanged = entry.IsChanged,
            ChangeSummary = entry.ChangeSummaryData != null && entry.ChangeSummaryData.Any() 
                ? JsonSerializer.Serialize(entry.ChangeSummaryData, jsonOptions) 
                : null
        };
    }

    private string? GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return "SYSTEM";
        
  
        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring(7);
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                    
                    if (!string.IsNullOrEmpty(userId))
                        return userId;
                }
            }
            catch
            {

            }
        }
        
        var userIdFromClaims = httpContext.User?.FindFirst("sub")?.Value
                     ?? httpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (!string.IsNullOrEmpty(userIdFromClaims))
            return userIdFromClaims;
        
        if (string.IsNullOrEmpty(authHeader))
        {
            return "api-user";
        }
        
        var apiKey = httpContext.Request.Headers["X-Api-Key"].FirstOrDefault();
        if (!string.IsNullOrEmpty(apiKey))
        {
            return $"API_{apiKey.Substring(0, Math.Min(8, apiKey.Length))}...";
        }
        
        var apiToken = httpContext.User?.FindFirst("QuickCodeApiToken")?.Value;
        if (!string.IsNullOrEmpty(apiToken))
        {
            return $"API_TOKEN_{apiToken.Substring(0, Math.Min(8, apiToken.Length))}...";
        }

        return "SYSTEM";
    }

    private string? GetCurrentUserName()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return "SYSTEM";
        
        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring(7);
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                    
                    if (!string.IsNullOrEmpty(userName))
                        return userName;
                }
            }
            catch
            {
            }
        }
        
        var userNameFromClaims = httpContext.User?.Identity?.Name
                       ?? httpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        
        if (!string.IsNullOrEmpty(userNameFromClaims))
            return userNameFromClaims;
        
        if (string.IsNullOrEmpty(authHeader))
        {
            return "API User";
        }
        
        var apiKey = httpContext.Request.Headers["X-Api-Key"].FirstOrDefault();
        if (!string.IsNullOrEmpty(apiKey))
        {
            return "API_CLIENT";
        }

        return "SYSTEM";
    }

    private string? GetCurrentUserGroup()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return "SYSTEM";
        
        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring(7);
            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "PermissionGroupName")?.Value;
                    
                    if (!string.IsNullOrEmpty(userName))
                        return userName;
                }
            }
            catch
            {
            }
        }
        
        var userGroupFromClaims = httpContext.User?.FindFirst("PermissionGroupName")?.Value;
        
        if (!string.IsNullOrEmpty(userGroupFromClaims))
            return userGroupFromClaims;
        
        if (string.IsNullOrEmpty(authHeader))
        {
            return "API User Group";
        }
        
        var apiKey = httpContext.Request.Headers["X-Api-Key"].FirstOrDefault();
        if (!string.IsNullOrEmpty(apiKey))
        {
            return "API_CLIENT_GROUP";
        }

        return "SYSTEM";
    }
    
    private string? GetIpAddress()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return null;
        
        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        return httpContext.Connection.RemoteIpAddress?.ToString();
    }

    private string? GetUserAgent()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return null;
        }

        var userAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(userAgent))
        {
            return userAgent;
        }

        var forwardedUserAgent = httpContext.Request.Headers["X-Forwarded-User-Agent"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwardedUserAgent))
        {
            return forwardedUserAgent;
        }

        var apiKey = httpContext.Request.Headers["X-Api-Key"].FirstOrDefault();
        if (!string.IsNullOrEmpty(apiKey))
        {
            return "API Client";
        }

        return "Unknown";
    }

    private string? GetCorrelationId()
    {
        return _httpContextAccessor.HttpContext?.TraceIdentifier
               ?? _httpContextAccessor.HttpContext?.Request.Headers["X-Correlation-ID"].FirstOrDefault();
    }

    private static string GetPrimaryKeyValue(EntityEntry entry)
    {
        var keyValues = entry.Properties
            .Where(p => p.Metadata.IsPrimaryKey())
            .Select(p => p.CurrentValue?.ToString() ?? "null");

        return string.Join(",", keyValues);
    }
}