namespace Core.RateLimiting;

public class DefaultFixedWindowRateLimitingOptions
{
    public required int PermitLimit { get; init; }

    public required TimeSpan Window { get; init; }
}