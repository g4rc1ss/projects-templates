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

    public async Task<UserEntity?> GetByIdAsync(string userId)
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

    public async Task<UserEntity?> GetUserByUserNameAsync(string userName)
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
    Task<UserEntity?> GetUserByUserNameAsync(string userName);
}