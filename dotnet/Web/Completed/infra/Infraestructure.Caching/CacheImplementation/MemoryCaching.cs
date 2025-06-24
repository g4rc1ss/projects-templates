using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace Infraestructure.Caching.CacheImplementation;

public class MemoryCaching(IMemoryCache memoryCache) : ICaching
{
    public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
    {
        using ActivitySource? tracingConsumer = new(InfraCachingExtensions.CACHING_TRACE);
        using Activity? activity = tracingConsumer.CreateActivity(
            $"Get object",
            ActivityKind.Producer
        );
        activity?.Start();
        return Task.FromResult(memoryCache.Get<byte[]>(key));
    }

    public Task SetAsync(
        string key,
        byte[] value,
        CachingOptions? options,
        CancellationToken token = default
    )
    {
        using ActivitySource? tracingConsumer = new(InfraCachingExtensions.CACHING_TRACE);
        using Activity? activity = tracingConsumer.CreateActivity(
            $"Set object",
            ActivityKind.Producer
        );
        activity?.Start();

        if (options?.AbsoluteExpirationRelativeToNow is not null)
        {
            memoryCache.Set(key, value, options.AbsoluteExpirationRelativeToNow.Value);
            return Task.CompletedTask;
        }

        if (options?.AbsoluteExpiration is not null)
        {
            memoryCache.Set(key, value, options.AbsoluteExpiration.Value);
            return Task.CompletedTask;
        }

        memoryCache.Set(key, value);
        return Task.CompletedTask;
    }

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        using ActivitySource? tracingConsumer = new(InfraCachingExtensions.CACHING_TRACE);
        using Activity? activity = tracingConsumer.CreateActivity(
            $"Refresh Cache",
            ActivityKind.Producer
        );
        activity?.Start();

        memoryCache.TryGetValue(key, out _);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        using ActivitySource? tracingConsumer = new(InfraCachingExtensions.CACHING_TRACE);
        using Activity? activity = tracingConsumer.CreateActivity(
            $"Remove Cache",
            ActivityKind.Producer
        );
        activity?.Start();

        memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}
