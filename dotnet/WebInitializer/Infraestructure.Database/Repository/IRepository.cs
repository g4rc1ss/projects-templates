namespace Infraestructure.Database.Repository;

public interface IRepository<TEntity>
    where TEntity : class
{
    TEntity? Entity { get; internal set; }
    Task<TEntity?> GetByIdAsync(string id);
}
