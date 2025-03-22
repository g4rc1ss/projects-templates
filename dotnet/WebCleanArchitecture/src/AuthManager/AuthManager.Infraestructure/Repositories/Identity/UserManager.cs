using AuthManager.Domain.BusinessObjects;
using AuthManager.Domain.InfraestructureContracts;
using AuthManager.Infraestructure.Mappers;
using Infraestructure.AuthManagerDB.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthManager.Infraestructure.Repositories.Identity;

public class UserManager(
    IUserInfo userInfo,
    UserManager<UserEntity> userManager
) : IUserManager
{
    public async Task<UserData?> GetUserByIdAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserEntity? user = await userInfo.GetUserByIdAsync(userId);

        return user?.ToUserData();
    }

    public async Task<UserData?> GetUserByUserNameAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);

        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        return user?.ToUserData();
    }

    public async Task<bool> UserExistsAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);

        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        return user is not null;
    }

    public Task<IList<string>> GetUserRolesAsync(int userId)
    {
        UserEntity? entity = new() { Id = userId };
        return userManager.GetRolesAsync(entity);
    }

    public async Task<bool> IsLockedAsync(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);


        return await userManager.IsLockedOutAsync(user);
    }

    public async Task<bool> CreateAsync(string userName, string password, string email)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(email);

        UserEntity user = new()
        {
            UserName = userName,
            Email = email
        };

        IdentityResult result = await userManager.CreateAsync(user, password);

        return result.Succeeded;
    }
}