using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;

namespace QuickCode.MyecommerceDemo.Common.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly PartitionedRateLimiter<HttpContext> _rateLimiter =
        PartitionedRateLimiter.Create<HttpContext, string>(
            context => RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    AutoReplenishment = true
                }));

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var lease = await _rateLimiter.AcquireAsync(context);

        if (lease.IsAcquired)
        {
            try
            {
                await _next(context);
            }
            finally
            {
                lease.Dispose();
            }
        }
        else
        {
            context.Response.StatusCode = 429; // Too Many Requests
            await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
        }
    }
}

public static class RateLimitingMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitingMiddleware>();
    }
}
