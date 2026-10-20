using System.Threading.RateLimiting;
using Core.RateLimiting.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace Core.RateLimiting;

public static class Installer
{
    public static ControllerActionEndpointConventionBuilder UseApiGuardDefaultRateLimiting(
        this ControllerActionEndpointConventionBuilder builder)
    {
        builder.Add(endpointBuilder =>
        {
            if (!endpointBuilder.Metadata.OfType<EnableRateLimitingAttribute>().Any())
            {
                endpointBuilder.Metadata.Add(
                    new EnableRateLimitingAttribute(Constants.ApiGuardDefaultPolicyName));
            }
        });

        return builder;
    }

    internal static IServiceCollection AddDefaultSlidingWindowRateLimitingPolicy(
        this IServiceCollection services,
        ApiGuardRateLimitingOptions rateLimitingOptions)
    {
        return services.AddSlidingWindowRateLimitingPolicy(
            Constants.ApiGuardDefaultPolicyName,
            rateLimitingOptions);
    }
    
    public static IServiceCollection AddSlidingWindowRateLimitingPolicy(
        this IServiceCollection services,
        string policyName,
        ApiGuardRateLimitingOptions rateLimitingOptions)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(policyName, _ =>
                RateLimitPartition.Get(policyName, _ =>
                {
                    var userLimiter = CreateLimiter(rateLimitingOptions.User);
                    var ipLimiter = CreateLimiter(rateLimitingOptions.Ip);
                    var sharedLimiter = CreateLimiter(rateLimitingOptions.Shared);
                    
                    return RateLimiter.CreateChained(userLimiter, ipLimiter, sharedLimiter);

                }));
        });

        return services;
    }
    
    private static RateLimiter CreateLimiter(
        SlidingWindowRateLimitingOptions options)
    {
        return new SlidingWindowRateLimiter(
            new SlidingWindowRateLimiterOptions
            {
                PermitLimit = options.PermitLimit,
                Window = options.Window
            });
    }
}