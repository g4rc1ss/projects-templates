namespace SimpleWeb.AspireHost.Resources;

public static class MongodbResource
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

    internal static void WithAspireMongodb<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<MongoDBServerResource> mongodb
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        builder.WithReference(mongodb).WaitFor(mongodb);
    }
}
