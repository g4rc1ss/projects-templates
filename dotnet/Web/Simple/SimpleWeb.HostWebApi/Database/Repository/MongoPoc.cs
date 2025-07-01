using MongoDB.Driver;
using SimpleWeb.HostWebApi.Database.Entities;

namespace SimpleWeb.HostWebApi.Database.Repository;

public class MongoPoc(ILogger<MongoPoc> logger, IMongoClient mongoClient) : IMongoPoc
{
    public MongoDbEntity? Entity { get; set; }

    public async Task<MongoDbEntity?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        if (Entity is not null && Entity.Id == id)
        {
            return Entity;
        }

        IMongoCollection<MongoDbEntity> collection = mongoClient
            .GetDatabase("Poc")
            .GetCollection<MongoDbEntity>("Entity");

        IAsyncCursor<MongoDbEntity> cursor = await collection.FindAsync(
            x => x.Id == id,
            cancellationToken: cancellationToken
        );

        while (await cursor.MoveNextAsync(cancellationToken))
        {
            logger.LogInformation("Devolvemos la entidad {DocumentId}", cursor.Current.First().Id);

            Entity = cursor.Current.First();
            return Entity;
        }

        throw new Exception("La entidad no existe");
    }

    public async Task<MongoDbEntity> CreateAsync(
        MongoDbEntity mongoDbEntity,
        CancellationToken cancellationToken = default
    )
    {
        IMongoCollection<MongoDbEntity> collection = mongoClient
            .GetDatabase("Poc")
            .GetCollection<MongoDbEntity>("Entity");

        await collection.InsertOneAsync(mongoDbEntity, cancellationToken: cancellationToken);

        Entity = mongoDbEntity;
        return Entity;
    }

    public Task UpdateAsync(
        MongoDbEntity mongoDbEntity,
        CancellationToken cancellationToken = default
    )
    {
        IMongoCollection<MongoDbEntity> collection = mongoClient
            .GetDatabase("Poc")
            .GetCollection<MongoDbEntity>("Entity");
        return collection.ReplaceOneAsync(
            x => x.Id == mongoDbEntity.Id,
            mongoDbEntity,
            cancellationToken: cancellationToken
        );
    }

    public Task DeleteAsync(
        MongoDbEntity mongoDbEntity,
        CancellationToken cancellationToken = default
    )
    {
        IMongoCollection<MongoDbEntity> collection = mongoClient
            .GetDatabase("Poc")
            .GetCollection<MongoDbEntity>("Entity");
        return collection.DeleteOneAsync(
            x => x.Id == mongoDbEntity.Id,
            cancellationToken: cancellationToken
        );
    }
}

public interface IMongoPoc : IRepository<MongoDbEntity, string>;
