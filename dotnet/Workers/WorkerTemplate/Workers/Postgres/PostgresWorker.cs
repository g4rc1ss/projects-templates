using System.Diagnostics;

namespace WorkerTemplate.Workers.Postgres;

public class PostgresWorker(
    ILogger<Worker> logger,
    IConfiguration configuration,
    PostgresDbContext dbContext
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using ActivitySource activitySource = new(nameof(PostgresWorker));
        activitySource.StartActivity();

        return Task.CompletedTask;
    }
}