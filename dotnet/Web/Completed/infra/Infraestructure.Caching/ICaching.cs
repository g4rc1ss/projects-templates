namespace Infraestructure.Caching;

public interface ICaching
{
    Task<byte[]?> GetAsync(string key, CancellationToken token = default);

    Task SetAsync(
        string key,
        byte[] value,
        CachingOptions? options = null,
        CancellationToken token = default
    );

    Task RefreshAsync(string key, CancellationToken token = default);
    Task RemoveAsync(string key, CancellationToken token = default);
}
