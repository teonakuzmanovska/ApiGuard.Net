using Core.ErrorHandling;
using Core.RateLimiting;
using Core.RateLimiting.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Installer
{
    public static IServiceCollection AddApiGuard(
        this IServiceCollection services,
        ApiGuardRateLimitingOptions rateLimitingOptions)
    {
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddProblemDetails();
        services.AddDefaultSlidingWindowRateLimitingPolicy(rateLimitingOptions);

        return services;
    }
    
    public static IApplicationBuilder UseApiGuard(
        this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        app.UseRateLimiter();

        return app;
    }
}