# ApiGuard.Net

## Introduction

ApiGuard.Net is a .NET 10 library for ASP.NET Core Web APIs that provides request validation, safe error handling and configurable rate limiting.

It supports fluent, fail-fast validation with custom conditions, consistent ProblemDetails responses and a default rate limiting policy that individual endpoints can override.

Error handling and rate limiting use ASP.NET Core's built-in infrastructure.

## Prerequisites

- .NET 10
- An ASP.NET Core Web API using controllers

The developer should provide the default rate limiting configuration:

- Number of requests allowed per window (`PermitLimit`)
- Duration of the window (`Window`)

## Installation

NuGet publication is pending. For now, add a project reference to `Core/Core.csproj`.

## Configuration

Add the following to `Program.cs`:

```csharp
using Core;
using Core.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var rateLimitingOptions = new DefaultFixedWindowRateLimitingOptions
{
    PermitLimit = 3,
    Window = TimeSpan.FromSeconds(10)
};

builder.Services.AddApiGuard(rateLimitingOptions);

var app = builder.Build();

app.UseApiGuard();
app.UseHttpsRedirection();

app.MapControllers()
    .UseApiGuardDefaultRateLimiting();

app.Run();
```

ApiGuard requires three calls:

- `AddApiGuard` registers the exception handler, ProblemDetails services and default rate limiting policy.
- `UseApiGuard` adds exception handling and rate limiting middleware, in that order.
- `UseApiGuardDefaultRateLimiting` applies the default policy to controller endpoints without an explicit named policy.

These are ApiGuard extensions. `AddControllers`, `MapControllers` and `UseHttpsRedirection` are built-in ASP.NET Core methods.

## Features

### Request validation

ApiGuard's `Validate` class checks properties through a fluent chain. The first failed check records an error. Remaining checks are skipped.

In a controller action, use:

```csharp
using Core.Validations.Extensions;
using Core.Validations.ValidationChain;

// request.Name is string? and request.Age is int?.
var validationResult = new Validate()
    .IsNotNullOrWhitespace(request.Name, nameof(request.Name))
    .IsNotNull(request.Age, nameof(request.Age))
    .Result();

if (!validationResult.IsValid)
{
    return validationResult.Error.ToBadRequest();
}
```

Available checks:

- `IsNotNull`: rejects null values.
- `IsNotNullOrWhitespace`: rejects null, empty or whitespace strings.
- `IsNotNullOrEmptyList`: rejects null or empty sequences.
- `IsExactType`: checks that the runtime type exactly matches the supplied type.
- `IsAssignableTo`: checks compatibility with the supplied type, including inheritance and interfaces.
- `IsSatisfied`: evaluates a custom condition.

Create a new `Validate` instance for each chain. For nullable values, call `IsNotNull` before `IsExactType`.

`Result()` returns a `ValidationResult` containing:

- `IsValid`: whether validation passed.
- `Error`: null on success, otherwise a `ValidationError` with `PropertyName` and `ErrorMessage`.

The compiler recognizes that `Error` is non-null when `IsValid` is false.

`ToBadRequest` is an ApiGuard extension that constructs a `400` ProblemDetails response. The developer does not need to construct the response body.

For example, an empty name produces a response containing:

```json
{
  "title": "String property must not be null, empty or whitespace.",
  "status": 400,
  "propertyName": "Name"
}
```

ASP.NET Core may include additional ProblemDetails fields.

### Custom validation

Add custom conditions using `IsSatisfied`:

```csharp
var validationResult = new Validate()
    .IsNotNull(request.Age, nameof(request.Age))
    .IsSatisfied(
        () => request.Age >= 18,
        nameof(request.Age),
        "Age must be at least 18.")
    .Result();
```

The method accepts a .NET `Func<bool>` delegate, the property name and an error message. It evaluates the delegate only if previous checks passed.

The developer can also create reusable extension methods that call `IsSatisfied`. Exceptions thrown by a condition propagate to the caller.

### Safe error handling

ApiGuard's `ApiExceptionHandler` implements ASP.NET Core's built-in `IExceptionHandler`.

Unexpected exceptions return a generic `500` ProblemDetails response:

```json
{
  "title": "An unexpected error occurred.",
  "status": 500
}
```

Internal exception messages and stack traces are not included in the response body.

ApiGuard also supports throwing its `ApiValidationException`. The exception handler returns a `400` response containing the validation message and property name:

```csharp
using Core.Validations.Exceptions;

if (!validationResult.IsValid)
{
    throw new ApiValidationException(validationResult.Error);
}
```

### Rate limiting

ApiGuard uses ASP.NET Core's fixed-window rate limiter. Requests exceeding the configured limit receive `429 Too Many Requests`.

The default policy is named `ApiGuardDefault`. The configuration above allows three requests per ten-second window.

Limits are shared across requests using the same policy. They are not applied separately per user or IP address.

Register additional policies before `builder.Build()`:

```csharp
builder.Services.AddFixedWindowRateLimitingPolicy(
    "policy1", 1, TimeSpan.FromSeconds(10));

builder.Services.AddFixedWindowRateLimitingPolicy(
    "policy2", 5, TimeSpan.FromSeconds(10));
```

Apply a named policy using ASP.NET Core's built-in attribute:

```csharp
using Microsoft.AspNetCore.RateLimiting;

[HttpGet("limited")]
[EnableRateLimiting("policy2")]
public IActionResult Limited() => Ok();
```

An explicit named policy replaces ApiGuard's default policy for that endpoint. The default is not enforced alongside it.

## Sample

Run the sample from the repository root:

```shell
dotnet run --project Sample/Sample.csproj
```

Open [Sample/Sample.http](Sample/Sample.http) to try request validation and safe error handling. Adjust `Sample_HostAddress` to match the application's address if needed.

With the sample running, run the PowerShell scripts from the repository root to verify rate limiting:

```powershell
.\Sample\Scripts\RateLimitDefaultPolicy.ps1
.\Sample\Scripts\RateLimitPolicy1.ps1
.\Sample\Scripts\RateLimitPolicy2.ps1
```

Each script sends rapid requests and prints the response status codes. In a fresh window, expected results are:

- Default policy: three `200` responses, followed by `429`.
- `policy1`: one `200` response, followed by `429`.
- `policy2`: five `200` responses, followed by `429`.

The scripts use `http://localhost:5069`. Update their URLs if the sample runs at a different address.

The sample's default limit is three requests per ten seconds. Wait for the next window if validation requests receive `429`.
