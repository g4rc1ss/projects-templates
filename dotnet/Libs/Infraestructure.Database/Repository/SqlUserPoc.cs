using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Database.Repository;

public class SqlUserPoc(DatabaseContext dbContext) : IUserRepository
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
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public interface IUserRepository : IRepository<UserEntity, int>;
