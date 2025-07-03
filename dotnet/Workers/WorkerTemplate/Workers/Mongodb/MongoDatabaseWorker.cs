using System.Diagnostics;
using MongoDB.Driver;

namespace WorkerTemplate.Workers.Mongodb;

public class MongoDatabaseWorker(ILogger<Worker> logger, IMongoClient mongoClient)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using ActivitySource activitySource = new(nameof(MongoDatabaseWorker));
        using (activitySource.StartActivity())
        {
            IMongoCollection<Entity>? collection = mongoClient
                .GetDatabase("database")
                .GetCollection<Entity>("Entities");

            Entity entity = new() { Id = Guid.NewGuid().ToString(), Property = "Propiedad de Mongo" };
            await collection.InsertOneAsync(entity, new InsertOneOptions(), stoppingToken);
            logger.LogInformation("Documento {DocumentId} insertado", entity.Id);

            using IAsyncCursor<Entity>? result = await collection.FindAsync(
                x => x.Id == entity.Id,
                cancellationToken: stoppingToken
            );

            while (await result.MoveNextAsync(stoppingToken))
            {
                logger.LogInformation(
                    "Documento encontrado con la propiedad {Property}",
                    result.Current.First().Property
                );
            }
        }
    }
}
