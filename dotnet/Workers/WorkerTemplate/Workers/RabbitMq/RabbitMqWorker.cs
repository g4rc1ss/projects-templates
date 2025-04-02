namespace WorkerTemplate.Workers.RabbitMq;

public class RabbitMqWorker(
    ILogger<RabbitMqWorker> logger
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}