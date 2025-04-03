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
    private readonly IModel? _channel = connection.CreateModel();

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ArgumentNullException.ThrowIfNull(_channel);

        EventingBasicConsumer receiver = new(_channel);
        string? queue = configuration.GetSection("RabbitMq")["QueueName"];

        receiver.Received += HandleMessage;
        _channel.BasicQos(0, 10, false);
        _channel.BasicConsume(queue, false, receiver);

        return Task.CompletedTask;
    }

    private void HandleMessage(object ch, BasicDeliverEventArgs eventArgs)
    {
        string body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        logger.LogInformation("El body recibido de rabbit: {MessageBody}", body);
    }
}