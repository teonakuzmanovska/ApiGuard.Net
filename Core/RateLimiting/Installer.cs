using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace Core.RateLimiting;

public static class Installer
{
    public static IServiceCollection AddGlobalFixedWindowRateLimitingPolicy(
        this IServiceCollection services,
        int permitLimit,
        TimeSpan window)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                _ =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: "global",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = permitLimit,
                            Window = window
                        }));
        });
        
        return services;
    }
    
    public static IServiceCollection AddFixedWindowRateLimitingPolicy(
        this IServiceCollection services,
        string policyName,
        int permitLimit,
        TimeSpan window)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddFixedWindowLimiter(policyName, limiterOptions =>
            {
                limiterOptions.PermitLimit = permitLimit;
                limiterOptions.Window = window;
            });
        });

        return services;
    }
}