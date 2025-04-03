using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WorkerTemplate.Workers.RabbitMq;

public class RabbitMqWorker(
    ILogger<RabbitMqWorker> logger,
    IConfiguration configuration,
    IConnection connection
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IModel channel = connection.CreateModel();
        AsyncEventingBasicConsumer receiver = new(channel);
        string? queue = configuration.GetSection("RabbitMq")["QueueName"];

        receiver.Received += HandleMessage;
        channel.BasicQos(0, 10, false);
        channel.BasicConsume(queue, false, receiver);

        return Task.CompletedTask;
    }

    private Task HandleMessage(object ch, BasicDeliverEventArgs eventArgs)
    {
        string body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        logger.LogInformation("El body recibido de rabbit: {MessageBody}", body);

        return Task.CompletedTask;
    }
}