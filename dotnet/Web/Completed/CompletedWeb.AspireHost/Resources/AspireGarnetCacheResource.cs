namespace CompletedWeb.AspireHost.Resources;

public static class AspireGarnetCacheResource
{
    internal static IResourceBuilder<GarnetResource> AddGarnetCache(
        this IDistributedApplicationBuilder builder
    )
    {
        return builder.AddGarnet("Cache");
    }

    internal static IResourceBuilder<T> WithGarnetCache<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<GarnetResource> garnet
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(garnet).WaitFor(garnet);
    }
}
