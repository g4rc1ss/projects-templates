using AuthManager.Application.Contracts.InfraestructureContracts;
using AuthManager.Domain.BusinessObjects;
using Infraestructure.Database.Entities;
using Infraestructure.Database.Repository;
#if (UseCustomIdentity)
using AuthManager.Infraestructure.Mappers;
using Microsoft.AspNetCore.Identity;
#endif

namespace AuthManager.Infraestructure.Repositories.Identity;

public class UserManager(
#if (UseCustomIdentity)
    UserManager<UserEntity> userManager,
#endif
    IUserInfo userInfo
) : IUserManager
{
    public async Task<UserData?> GetUserByIdAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        UserEntity? user = await userInfo.GetByIdAsync(userId);

#if (UseCustomIdentity)
        return user?.ToUserData();
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<UserData?> GetUserByUserNameAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);

        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseCustomIdentity)
        return user?.ToUserData();
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> UserExistsAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);

        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        return user is not null;
    }

    public Task<IList<string>> GetUserRolesAsync(int userId)
    {
#if (UseCustomIdentity)
        UserEntity? entity = new() { Id = userId };
        return userManager.GetRolesAsync(entity);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> IsLockedAsync(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

#if (UseCustomIdentity)
        return await userManager.IsLockedOutAsync(user);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> CreateAsync(string userName, string password, string email)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(email);

#if (UseCustomIdentity)
        UserEntity user = new()
        {
            UserName = userName,
            Email = email
        };
        IdentityResult result = await userManager.CreateAsync(user, password);

        return result.Succeeded;
#else
        throw new NotImplementedException();
#endif
    }
}