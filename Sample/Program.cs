using Core.ErrorHandling;
using Core.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDefaultFixedWindowRateLimitingPolicy(3,TimeSpan.FromSeconds(10));
builder.Services.AddFixedWindowRateLimitingPolicy("policy1",1,TimeSpan.FromSeconds(10));
builder.Services.AddFixedWindowRateLimitingPolicy("policy2",5,TimeSpan.FromSeconds(10));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRateLimiter();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers()
    .UseApiGuardDefaultRateLimiting();

app.Run();