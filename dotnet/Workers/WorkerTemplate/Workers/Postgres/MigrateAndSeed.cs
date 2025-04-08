using Microsoft.EntityFrameworkCore;

namespace WorkerTemplate.Workers.Postgres;

public class MigrateAndSeed(
    ILogger<Worker> logger,
    IDbContextFactory<PostgresDbContext> dbContextFactory
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using PostgresDbContext context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await context.Database.MigrateAsync(cancellationToken);

        logger.LogInformation("Postgres migrated");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}