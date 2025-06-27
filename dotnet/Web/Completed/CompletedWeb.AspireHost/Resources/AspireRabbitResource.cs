namespace CompletedWeb.AspireHost.Resources;

public static class AspireRabbitResource
{
    internal static IResourceBuilder<RabbitMQServerResource> AddAspireRabbitMq(
        this IDistributedApplicationBuilder builder
    )
    {
        IResourceBuilder<ParameterResource> rabbitUsername = builder.AddParameter(
            "rabbitUser",
            "guest"
        );
        IResourceBuilder<ParameterResource> rabbitPassword = builder.AddParameter(
            "rabbitPass",
            "ERnyEKvg5mY1ByTDjHyey6"
        );

        IResourceBuilder<RabbitMQServerResource> rabbit = builder
            .AddRabbitMQ("RabbitMQ", rabbitUsername, rabbitPassword)
            .WithDataVolume("rabbitMQVM", isReadOnly: false)
            .WithBindMount("./Configs/RabbitMq/definitions.json", "/etc/rabbitmq/definitions.json")
            .WithBindMount("./Configs/RabbitMq/rabbitmq.conf", "/etc/rabbitmq/rabbitmq.conf")
            .WithLifetime(ContainerLifetime.Session)
            .WithManagementPlugin(58887);

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
