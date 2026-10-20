namespace Core.RateLimiting.Models;

public class SlidingWindowRateLimitingOptions
{
    public required int PermitLimit { get; init; }

    public required TimeSpan Window { get; init; }
}