namespace Infraestructure.Database.Repository;

public interface IRepository<TEntity, in TId>
    where TEntity : class
{
    TEntity? Entity { get; internal set; }
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
