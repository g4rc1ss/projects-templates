namespace CompletedWeb.AspireHost.Resources;

public static class AspireMongodbResource
{
    internal static IResourceBuilder<MongoDBServerResource> AddAspireMongo(
        this IDistributedApplicationBuilder builder
    )
    {
        return builder
            .AddMongoDB("mongo")
            // .WithDataVolume("MongoVM", isReadOnly: false)
            .WithLifetime(ContainerLifetime.Session)
            .WithMongoExpress();
    }

    internal static IResourceBuilder<T> WithAspireMongodb<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<MongoDBServerResource> mongodb
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(mongodb).WaitFor(mongodb);
    }
}
