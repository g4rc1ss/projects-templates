using Infraestructure.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthManager.Infraestructure.Repositories.Identity;

internal class UserInfo(
    UserManager<UserEntity> userManager
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

        return await userManager.FindByIdAsync(userId);
    }

    public async Task<UserEntity?> GetUserByUserNameAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);

        if (User is not null)
        {
            return User;
        }

        return await userManager.FindByNameAsync(userName);
    }
}

public interface IUserInfo
{
    Task<UserEntity?> GetUserByIdAsync(string userId);
    Task<UserEntity?> GetUserByUserNameAsync(string userName);
}