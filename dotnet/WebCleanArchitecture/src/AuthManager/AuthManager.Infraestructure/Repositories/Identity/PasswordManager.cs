using AuthManager.Domain.InfraestructureContracts;
using Infraestructure.Database.Entities;
using Infraestructure.Database.Repository;
using Microsoft.AspNetCore.Identity;

namespace AuthManager.Infraestructure.Repositories.Identity;

public class PasswordManager(
    IUserInfo userInfo,
    UserManager<UserEntity> userManager
) : IPasswordManager
{
    public async Task<bool> CheckPasswordAsync(string userName, string password)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(password);

        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        bool isCorrect = await userManager.CheckPasswordAsync(user, password);
        if (!isCorrect)
        {
            await userManager.AccessFailedAsync(user);
        }
        else
        {
            await userManager.ResetAccessFailedCountAsync(user);
        }

        return isCorrect;
    }

    public async Task<string> GetResetTokenAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<bool> ConfirmPasswordTokenAsync(string userName, string token, string password)
    {
        ArgumentNullException.ThrowIfNull(userName);
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        IdentityResult result = await userManager.ResetPasswordAsync(user, token, password);

        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        IdentityResult result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        return result.Succeeded;
    }
}