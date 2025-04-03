using MongoDB.Driver;

namespace WorkerTemplate.Workers.Mongodb;

public class MongoDatabaseWorker(
    ILogger<Worker> logger,
    IConfiguration configuration,
    IMongoClient mongoClient
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}