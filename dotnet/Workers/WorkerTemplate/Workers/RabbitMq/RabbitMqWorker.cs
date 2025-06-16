using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
        _channel.BasicConsume(queue, true, receiver);

        return Task.CompletedTask;
    }

    private void HandleMessage(object? ch, BasicDeliverEventArgs eventArgs)
    {
        using ActivitySource activitySource = new(nameof(RabbitMqWorker));
        activitySource.StartActivity();
        using Activity? activityProcess = activitySource.CreateActivity(
            "Process message",
            ActivityKind.Consumer
        );
        activityProcess?.Start();

        string body = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        logger.LogInformation("El body recibido de rabbit: {MessageBody}", body);

        activityProcess?.SetStatus(ActivityStatusCode.Ok);
        activityProcess?.Stop();

        Activity? activityAck = activitySource
            .CreateActivity("ACK", ActivityKind.Consumer)
            ?.Start();
        _channel?.BasicAck(eventArgs.DeliveryTag, false);
        activityAck?.Stop();
    }
}
