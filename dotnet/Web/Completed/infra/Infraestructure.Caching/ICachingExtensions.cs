using System.Text.Json;

namespace Infraestructure.Caching;

public static class CachingExtensions
{
    public static Task SetAsync<T>(
        this ICaching caching,
        string key,
        T value,
        CachingOptions options,
        CancellationToken token = default
    )
    {
        byte[] encodingValue = JsonSerializer.SerializeToUtf8Bytes(value);
        return caching.SetAsync(key, encodingValue, options, token);
    }

    public static Task SetAsync<T>(
        this ICaching caching,
        string key,
        T value,
        CancellationToken token = default
    )
    {
        byte[] encodingValue = JsonSerializer.SerializeToUtf8Bytes(value);
        return caching.SetAsync(key, encodingValue, token: token);
    }

    public static async Task<T?> GetAsync<T>(
        this ICaching caching,
        string key,
        CancellationToken token = default
    )
    {
        byte[]? encodingValue = await caching.GetAsync(key, token);
        return encodingValue is null ? default : JsonSerializer.Deserialize<T>(encodingValue);
    }
}
