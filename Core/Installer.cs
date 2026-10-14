using Core.ErrorHandling;
using Core.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Installer
{
    public static IServiceCollection AddApiGuard(
        this IServiceCollection services,
        DefaultFixedWindowRateLimitingOptions rateLimitingOptions)
    {
        services.AddExceptionHandler<ApiExceptionHandler>();
        services.AddProblemDetails();
        services.AddDefaultFixedWindowRateLimitingPolicy(rateLimitingOptions.PermitLimit, rateLimitingOptions.Window);

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