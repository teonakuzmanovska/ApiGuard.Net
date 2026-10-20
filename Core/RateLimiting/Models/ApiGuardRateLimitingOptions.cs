namespace Core.RateLimiting.Models;

public class ApiGuardRateLimitingOptions
{
    public required SlidingWindowRateLimitingOptions User { get; init; }
    
    public required SlidingWindowRateLimitingOptions Ip { get; init; }

    public required SlidingWindowRateLimitingOptions Shared { get; init; }
}