using AuthManager.Application.Contracts.InfraestructureContracts;
using Infraestructure.Database.Entities;
#if (UseIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace AuthManager.Infraestructure.Repositories.Identity;

public class EmailManager(
#if (UseIdentity)
    UserManager<UserEntity> userManager,
#endif
    IUserInfo userInfo
) : IEmailManager
{
    public async Task<bool> CheckEmailConfirmAsync(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        return await userManager.IsEmailConfirmedAsync(user);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> ConfirmEmailAsync(string userName, string token)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        IdentityResult result = await userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        return await userManager.GenerateEmailConfirmationTokenAsync(user);
#else
        throw new NotImplementedException();
#endif
    }
}