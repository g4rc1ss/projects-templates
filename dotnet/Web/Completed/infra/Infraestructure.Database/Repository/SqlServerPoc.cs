using Infraestructure.Database.Contexts;
using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infraestructure.Database.Repository;

public class SqlServerPoc(SqlServerContext dbContext) : ISqlServerPoc
{
    public UserEntity? Entity { get; set; }

    public async Task<UserEntity?> GetByIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    )
    {
        if (Entity is not null)
        {
            return Entity;
        }

        UserEntity user = await dbContext.Users.FirstAsync(
            entity => entity.Id == userId,
            cancellationToken
        );

        Entity = user;

        return user;
    }

    public async Task<UserEntity> CreateAsync(
        UserEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        EntityEntry<UserEntity> user = await dbContext.Users.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return user.Entity;
    }

    public async Task CreateManyAsync(
        IEnumerable<UserEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        await dbContext.Users.AddRangeAsync(entities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateManyAsync(
        IEnumerable<UserEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.Users.UpdateRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteManyAsync(
        IEnumerable<UserEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.Users.RemoveRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

public interface ISqlServerPoc : IRepository<UserEntity, int>, IManyCommandRepository<UserEntity>;
