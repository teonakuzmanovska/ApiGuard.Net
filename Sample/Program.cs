using Core;
using Core.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var defaultFixedWindowRateLimitingOptions = new DefaultFixedWindowRateLimitingOptions
{
    PermitLimit = 3,
    Window = TimeSpan.FromSeconds(10)
};

builder.Services.AddApiGuard(defaultFixedWindowRateLimitingOptions);

builder.Services.AddFixedWindowRateLimitingPolicy("policy1",1,TimeSpan.FromSeconds(10));
builder.Services.AddFixedWindowRateLimitingPolicy("policy2",5,TimeSpan.FromSeconds(10));

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