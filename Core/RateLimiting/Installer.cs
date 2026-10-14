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

    public static IServiceCollection AddDefaultFixedWindowRateLimitingPolicy(
        this IServiceCollection services,
        int permitLimit,
        TimeSpan window)
    {
        return services.AddFixedWindowRateLimitingPolicy(
            Constants.ApiGuardDefaultPolicyName,
            permitLimit,
            window);
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