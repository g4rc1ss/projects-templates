namespace SimpleWeb.AspireHost.Resources;

public static class GarnetCacheResource
{
    internal static IResourceBuilder<GarnetResource> AddGarnetCache(
        this IDistributedApplicationBuilder builder
    )
    {
        return builder.AddGarnet("Cache");
    }

    internal static void WithGarnetCache<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<GarnetResource> redis
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        builder.WithReference(redis).WaitFor(redis);
    }
}
