using Microsoft.Extensions.Caching.Distributed;

namespace Infraestructure.Caching.CacheImplementation;

public class DistributedCaching(IDistributedCache cache) : ICaching
{
    public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        return cache.GetAsync(key, token);
    }

    public Task SetAsync(
        string key,
        byte[] value,
        CachingOptions? options,
        CancellationToken token = default
    )
    {
        if (options is null)
        {
            return cache.SetAsync(key, value, token);
        }

        DistributedCacheEntryOptions cacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
            AbsoluteExpiration = options.AbsoluteExpiration,
            SlidingExpiration = options.SlidingExpiration,
        };
        return cache.SetAsync(key, value, cacheOptions, token);
    }

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        return cache.RefreshAsync(key, token);
    }

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        return cache.RemoveAsync(key, token);
    }
}
