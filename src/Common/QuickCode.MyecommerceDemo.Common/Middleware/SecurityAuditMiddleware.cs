using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace QuickCode.MyecommerceDemo.Common.Middleware;

public class SecurityAuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityAuditMiddleware> _logger;

    public SecurityAuditMiddleware(RequestDelegate next, ILogger<SecurityAuditMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            // Audit trail başlat
            var auditInfo = new SecurityAuditInfo
            {
                Timestamp = startTime,
                UserId = GetUserId(context),
                UserAgent = context.Request.Headers["User-Agent"].ToString(),
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                Method = context.Request.Method,
                Path = context.Request.Path.ToString(),
                QueryString = context.Request.QueryString.ToString(),
                IsAuthenticated = context.User.Identity?.IsAuthenticated ?? false,
                Claims = context.User.Claims.Select(c => new ClaimInfo { Type = c.Type, Value = c.Value }).ToList()
            };

            await _next(context);

            // Audit trail tamamla
            auditInfo.StatusCode = context.Response.StatusCode;
            auditInfo.Duration = DateTime.UtcNow - startTime;
            auditInfo.ResponseSize = context.Response.ContentLength ?? 0;

            LogSecurityAudit(auditInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Security audit error for {Path}", context.Request.Path);
            throw;
        }
    }

    private static string GetUserId(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim?.Value ?? "anonymous";
    }

    private void LogSecurityAudit(SecurityAuditInfo auditInfo)
    {
        var logLevel = auditInfo.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
        
        _logger.Log(logLevel, 
            "Security Audit: {Method} {Path} - User: {UserId} - IP: {IpAddress} - Status: {StatusCode} - Duration: {Duration}ms",
            auditInfo.Method, auditInfo.Path, auditInfo.UserId, auditInfo.IpAddress, 
            auditInfo.StatusCode, auditInfo.Duration.TotalMilliseconds);

        // Kritik güvenlik olayları için özel loglama
        if (IsCriticalSecurityEvent(auditInfo))
        {
            _logger.LogWarning("CRITICAL SECURITY EVENT: {EventDetails}", auditInfo);
        }
    }

    private static bool IsCriticalSecurityEvent(SecurityAuditInfo auditInfo)
    {
        return auditInfo.StatusCode == 401 || 
               auditInfo.StatusCode == 403 || 
               auditInfo.Path.Contains("/admin") ||
               auditInfo.Path.Contains("/api/auth");
    }
}

public class SecurityAuditInfo
{
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string QueryString { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
    public List<ClaimInfo> Claims { get; set; } = new();
    public int StatusCode { get; set; }
    public TimeSpan Duration { get; set; }
    public long ResponseSize { get; set; }
}

public class ClaimInfo
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public static class SecurityAuditMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityAudit(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityAuditMiddleware>();
    }
} 