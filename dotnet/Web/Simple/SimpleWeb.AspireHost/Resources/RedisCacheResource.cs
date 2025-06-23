namespace SimpleWeb.AspireHost.Resources;

public static class RedisCacheResource
{
    internal static IResourceBuilder<RedisResource> AddRedisCache(
        this IDistributedApplicationBuilder builder
    )
    {
        return builder.AddRedis("Cache").WithRedisCommander().WithRedisInsight();
    }

    internal static void WithRedisCache<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<RedisResource> redis
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        builder.WithReference(redis);
        builder.WaitFor(redis);
    }
}
