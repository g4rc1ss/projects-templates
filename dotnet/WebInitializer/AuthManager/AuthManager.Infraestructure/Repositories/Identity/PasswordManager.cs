using AuthManager.Application.Contracts.InfraestructureContracts;
using Infraestructure.Database.Entities;
using Infraestructure.Database.Repository;
#if (UseCustomIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace AuthManager.Infraestructure.Repositories.Identity;

public class PasswordManager(
#if (UseCustomIdentity)
    UserManager<UserEntity> userManager,
#endif
    IUserInfo userInfo
) : IPasswordManager
{
    public async Task<bool> CheckPasswordAsync(string userName, string password)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(password);

        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseCustomIdentity)
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
#else
        // Comprobar si la pass es correcta
        // Si no es correcta, implementar el contador de login erroneos
        throw new NotImplementedException();
#endif
    }

    public async Task<string> GetResetTokenAsync(string userName)
    {
        ArgumentNullException.ThrowIfNull(userName);
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseCustomIdentity)
        return await userManager.GeneratePasswordResetTokenAsync(user);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> ConfirmPasswordTokenAsync(string userName, string token, string password)
    {
        ArgumentNullException.ThrowIfNull(userName);
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseCustomIdentity)
        IdentityResult result = await userManager.ResetPasswordAsync(user, token, password);

        return result.Succeeded;
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseCustomIdentity)
        IdentityResult result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        return result.Succeeded;
#else
        throw new NotImplementedException();
#endif
    }
}