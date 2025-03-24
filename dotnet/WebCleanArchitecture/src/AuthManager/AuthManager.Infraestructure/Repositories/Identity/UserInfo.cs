using Infraestructure.Database.Entities;
#if (UseIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace AuthManager.Infraestructure.Repositories.Identity;

internal class UserInfo(

#if (UseIdentity)
    UserManager<UserEntity> userManager
#endif
) : IUserInfo
{
    public UserEntity? User { get; set; }

    public async Task<UserEntity?> GetUserByIdAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        if (User is not null)
        {
            return User;
        }
#if (UseIdentity)
        return await userManager.FindByIdAsync(userId);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<UserEntity?> GetUserByUserNameAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);

        if (User is not null)
        {
            return User;
        }
#if (UseIdentity)
        return await userManager.FindByNameAsync(userName);
#else
        throw new NotImplementedException();
#endif
    }
}

public interface IUserInfo
{
    Task<UserEntity?> GetUserByIdAsync(string userId);
    Task<UserEntity?> GetUserByUserNameAsync(string userName);
}