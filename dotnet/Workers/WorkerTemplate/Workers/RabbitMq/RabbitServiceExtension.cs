namespace WorkerTemplate.Workers.RabbitMq;

public static class RabbitServiceExtension
{
    internal static void AddRabbitMq(this IHostApplicationBuilder builder)
    {
        builder.AddRabbitMQClient("RabbitMQ");
        builder.Services.AddHostedService<RabbitMqWorker>();
    }
}
