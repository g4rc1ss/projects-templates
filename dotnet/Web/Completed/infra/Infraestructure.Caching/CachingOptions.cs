namespace Infraestructure.Caching;

public record CachingOptions(
    DateTimeOffset? AbsoluteExpiration,
    TimeSpan? SlidingExpiration,
    TimeSpan? AbsoluteExpirationRelativeToNow
);
