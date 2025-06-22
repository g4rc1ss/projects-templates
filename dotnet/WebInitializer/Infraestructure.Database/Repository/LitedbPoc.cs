using Infraestructure.Database.Entities;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Infraestructure.Database.Repository;

public class LitedbPoc(ILogger<LitedbPoc> logger, ILiteDatabase litedb) : ILitedbPoc
{
    public LiteDbEntity? Entity { get; set; }

    public async Task<LiteDbEntity?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        if (Entity is not null && Entity.Id == id)
        {
            return Entity;
        }

        ILiteCollection<LiteDbEntity> collection = litedb.GetCollection<LiteDbEntity>("Entity");

        LiteDbEntity litedbEntity = collection.Query().Where(x => x.Id == id).First();

        logger.LogInformation("Devolvemos la entidad {documentId}", litedbEntity.Id);

        Entity = litedbEntity;
        return Entity;
    }

    public async Task<LiteDbEntity> CreateAsync(
        LiteDbEntity liteDbEntity,
        CancellationToken cancellationToken = default
    )
    {
        ILiteCollection<LiteDbEntity> collection = litedb.GetCollection<LiteDbEntity>("Entity");

        collection.Insert(liteDbEntity);

        Entity = liteDbEntity;
        return Entity;
    }

    public Task UpdateAsync(
        LiteDbEntity liteDbEntity,
        CancellationToken cancellationToken = default
    )
    {
        ILiteCollection<LiteDbEntity> collection = litedb.GetCollection<LiteDbEntity>("Entity");

        collection.Update(liteDbEntity.Id, liteDbEntity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        LiteDbEntity liteDbEntity,
        CancellationToken cancellationToken = default
    )
    {
        ILiteCollection<LiteDbEntity> collection = litedb.GetCollection<LiteDbEntity>("Entity");

        collection.Delete(liteDbEntity.Id);

        return Task.CompletedTask;
    }
}

public interface ILitedbPoc : IRepository<LiteDbEntity, string>;
