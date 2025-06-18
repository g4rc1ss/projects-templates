using Infraestructure.Database.Entities;
#if (UseIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace Infraestructure.Database.Repository;

public class UserInfo(
#if (UseIdentity)
    UserManager<UserEntity> userManager
#endif
) : IUserInfo
{
    public UserEntity? Entity { get; set; }

    public async Task<UserEntity?> GetByIdAsync(
        string userId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        if (Entity is not null)
        {
            return Entity;
        }
#if (UseIdentity)
        Entity = await userManager.FindByIdAsync(userId);
        return Entity;
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<UserEntity> CreateAsync(
        UserEntity entity,
        CancellationToken cancellationToken = default
    )
    {
#if (UseIdentity)
        IdentityResult result = await userManager.CreateAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception("Error creating user");
        }

        Entity = entity;
        return entity;

#else
        throw new NotImplementedException();
#endif
    }

    public async Task UpdateAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
#if (UseIdentity)
        IdentityResult result = await userManager.UpdateAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception("Error updating user");
        }
#else
        throw new NotImplementedException();
#endif
    }

    public async Task DeleteAsync(UserEntity entity, CancellationToken cancellationToken = default)
    {
#if (UseIdentity)
        IdentityResult result = await userManager.DeleteAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception("Error updating user");
        }
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<UserEntity?> GetUserByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userName);

        if (Entity is not null)
        {
            return Entity;
        }
#if (UseIdentity)
        Entity = await userManager.FindByNameAsync(userName);
        return Entity;
#else
        throw new NotImplementedException();
#endif
    }
}

public interface IUserInfo : IRepository<UserEntity>
{
    Task<UserEntity?> GetUserByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default
    );
}
