using AuthManager.Domain.InfraestructureContracts;
using Infraestructure.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthManager.Infraestructure.Repositories.Identity;

public class EmailManager(
    UserManager<UserEntity> userManager,
    IUserInfo userInfo
) : IEmailManager
{
    public async Task<bool> CheckEmailConfirmAsync(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        return await userManager.IsEmailConfirmedAsync(user);
    }

    public async Task<bool> ConfirmEmailAsync(string userName, string token)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        IdentityResult result = await userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        return await userManager.GenerateEmailConfirmationTokenAsync(user);
    }
}