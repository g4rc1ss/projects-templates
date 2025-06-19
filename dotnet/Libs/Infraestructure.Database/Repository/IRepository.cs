namespace Infraestructure.Database.Repository;

public interface IRepository<TEntity, in TId>
    : IQueryRepository<TEntity, TId>,
        ISingleCommandRepository<TEntity, TId>
    where TEntity : class
{
    TEntity? Entity { get; internal set; }
}

public interface IQueryRepository<TEntity, in TId>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
}

public interface ISingleCommandRepository<TEntity, in TId>
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}

public interface IManyCommandRepository<in TEntity>
{
    Task CreateManyAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    );
    Task UpdateManyAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    );
    Task DeleteManyAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default
    );
}
