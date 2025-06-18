namespace Infraestructure.Database.Repository;

public interface IRepository<TEntity>
    where TEntity : class
{
    TEntity? Entity { get; internal set; }
    Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
