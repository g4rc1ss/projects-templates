namespace WorkerTemplate.Workers;

public class AzServiceBusWorker(
    ILogger<AzServiceBusWorker> logger
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}