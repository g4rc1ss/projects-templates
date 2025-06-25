namespace CompletedWeb.AspireHost.Resources;

public static class AspireRabbitResource
{
    internal static IResourceBuilder<RabbitMQServerResource> AddAspireRabbitMq(
        this IDistributedApplicationBuilder builder
    )
    {
        IResourceBuilder<RabbitMQServerResource> rabbit = builder
            .AddRabbitMQ("RabbitMQ")
            .WithDataVolume("rabbitMQVM", isReadOnly: false)
            .WithLifetime(ContainerLifetime.Session)
            .WithManagementPlugin();

        return rabbit;
    }

    internal static IResourceBuilder<T> WithAspireRabbitMq<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<RabbitMQServerResource> rabbitMq
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(rabbitMq).WaitFor(rabbitMq);
    }
}
