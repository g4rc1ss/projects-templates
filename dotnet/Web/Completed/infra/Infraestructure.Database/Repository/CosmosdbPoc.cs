using Infraestructure.Database.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;

namespace Infraestructure.Database.Repository;

public class CosmosdbPoc(ILogger<CosmosdbPoc> logger, CosmosClient cosmosClient) : ICosmosdbPoc
{
    public CosmosDbEntity? Entity { get; set; }

    public async Task<CosmosDbEntity?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        if (Entity is not null && Entity.Id == id)
        {
            return Entity;
        }

        Container container = cosmosClient.GetDatabase("Database").GetContainer("Container");

        FeedIterator<CosmosDbEntity>? iterator = container
            .GetItemLinqQueryable<CosmosDbEntity>()
            .Where(e => e.Id == id)
            .ToFeedIterator();

        while (iterator.HasMoreResults)
        {
            FeedResponse<CosmosDbEntity> result = await iterator.ReadNextAsync(cancellationToken);
            logger.LogInformation("Devolvemos la entidad {documentId}", result.First().Id);

            Entity = result.First();
            return Entity;
        }

        throw new Exception("La entidad no existe");
    }

    public async Task<CosmosDbEntity> CreateAsync(
        CosmosDbEntity cosmosEntity,
        CancellationToken cancellationToken = default
    )
    {
        Container container = cosmosClient.GetDatabase("Database").GetContainer("Container");

        ItemResponse<CosmosDbEntity> response = await container.CreateItemAsync(
            cosmosEntity,
            cancellationToken: cancellationToken
        );

        Entity = response;
        return Entity;
    }

    public async Task UpdateAsync(
        CosmosDbEntity cosmosEntity,
        CancellationToken cancellationToken = default
    )
    {
        Container container = cosmosClient.GetDatabase("Database").GetContainer("Container");

        await container.UpsertItemAsync(cosmosEntity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(
        CosmosDbEntity cosmosEntity,
        CancellationToken cancellationToken = default
    )
    {
        Container container = cosmosClient.GetDatabase("Database").GetContainer("Container");

        await container.DeleteItemAsync<CosmosDbEntity>(
            cosmosEntity.Id,
            new PartitionKey(),
            cancellationToken: cancellationToken
        );
    }
}

public interface ICosmosdbPoc : IRepository<CosmosDbEntity, string>;
