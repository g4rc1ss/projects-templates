using Azure.Messaging.ServiceBus;

namespace WorkerTemplate.Workers.Postgres;

public class PostgresWorker(
    ILogger<Worker> logger,
    IConfiguration configuration,
    PostgresDbContext dbContext
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }


    private Task ErrorMessageAsync(ProcessErrorEventArgs args)
    {
        return Task.CompletedTask;
    }
}