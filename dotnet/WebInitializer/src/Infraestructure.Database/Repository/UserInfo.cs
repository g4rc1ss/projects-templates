using Infraestructure.Database.Entities;
#if (UseCustomIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace Infraestructure.Database.Repository;

public class UserInfo(

#if (UseCustomIdentity)
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
#if (UseCustomIdentity)
        return await userManager.FindByIdAsync(userId);
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
#if (UseCustomIdentity)
        return await userManager.FindByNameAsync(userName);
#else
        throw new NotImplementedException();
#endif
    }
}

public interface IUserInfo : IRepository<UserEntity>
{
    Task<UserEntity?> GetUserByUserNameAsync(string userName);
}