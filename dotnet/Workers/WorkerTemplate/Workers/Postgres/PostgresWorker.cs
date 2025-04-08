using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace WorkerTemplate.Workers.Postgres;

public class PostgresWorker(
    ILogger<Worker> logger,
    IDbContextFactory<PostgresDbContext> dbContextFactory
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using PostgresDbContext context = await dbContextFactory.CreateDbContextAsync(stoppingToken);

        using ActivitySource activitySource = new(nameof(PostgresWorker));
        Activity? activity = activitySource.StartActivity();

        await context.Database.MigrateAsync(stoppingToken);

        List<Entity> entities = [];
        entities.AddRange(Enumerable.Range(0, 1000)
            .Select(range => new Entity
            {
                Name = $"Entity-{range}",
            }));
        await context.AddRangeAsync(entities, stoppingToken);
        await context.SaveChangesAsync(stoppingToken);

        Entity? entity = await context.Entity.FirstOrDefaultAsync(x => x.Id == 1, stoppingToken);

        logger.LogInformation("Entity {EntityName} has been added.", entity?.Name);

        activity?.Stop();
    }
}