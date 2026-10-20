using Core;
using Core.RateLimiting;
using Core.RateLimiting.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var rateLimitingOptions = new ApiGuardRateLimitingOptions
{
    User = new()
    {
        PermitLimit = 3,
        Window = TimeSpan.FromSeconds(10)
    },
    Ip = new()
    {
        PermitLimit = 10,
        Window = TimeSpan.FromSeconds(10)
    },
    Shared = new()
    {
        PermitLimit = 100,
        Window = TimeSpan.FromSeconds(10)
    }
};

var rateLimitingOptionsP1 = new ApiGuardRateLimitingOptions
{
    User = new()
    {
        PermitLimit = 3,
        Window = TimeSpan.FromSeconds(10)
    },
    Ip = new()
    {
        PermitLimit = 10,
        Window = TimeSpan.FromSeconds(10)
    },
    Shared = new()
    {
        PermitLimit = 100,
        Window = TimeSpan.FromSeconds(10)
    }
};

var rateLimitingOptionsP2 = new ApiGuardRateLimitingOptions
{
    User = new()
    {
        PermitLimit = 3,
        Window = TimeSpan.FromSeconds(10)
    },
    Ip = new()
    {
        PermitLimit = 10,
        Window = TimeSpan.FromSeconds(10)
    },
    Shared = new()
    {
        PermitLimit = 100,
        Window = TimeSpan.FromSeconds(10)
    }
};

builder.Services.AddApiGuard(rateLimitingOptions);

builder.Services.AddSlidingWindowRateLimitingPolicy("policy1",rateLimitingOptionsP1);
builder.Services.AddSlidingWindowRateLimitingPolicy("policy2",rateLimitingOptionsP2);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseApiGuard();

app.UseHttpsRedirection();

app.MapControllers()
    .UseApiGuardDefaultRateLimiting();

app.Run();