namespace WorkerTemplate.Workers;

public class RabbitMqWorker(
    ILogger<RabbitMqWorker> logger
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}