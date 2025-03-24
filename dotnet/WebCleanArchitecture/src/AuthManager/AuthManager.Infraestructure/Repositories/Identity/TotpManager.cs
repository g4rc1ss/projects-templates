using AuthManager.Domain.InfraestructureContracts;
using Infraestructure.Database.Entities;
#if (UseIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace AuthManager.Infraestructure.Repositories.Identity;

public class TotpManager(
#if (UseIdentity)
    UserManager<UserEntity> userManager,
#endif
    IUserInfo userInfo
) : ITotpManager
{
    public async Task<bool> HasTwoFactorAuth(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        return await userManager.GetTwoFactorEnabledAsync(user);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> VerifyTotpCodeAsync(string userName, string? totpCode)
    {
        if (string.IsNullOrEmpty(totpCode))
        {
            return false;
        }

        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        return await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider,
            totpCode);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> VerifyTwoFactorTokenAsync(string userName, string totpCode)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        return await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, totpCode);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<IEnumerable<string>?> GenerateRecoveryCodesAsync(string userName, int numberOfCodes)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        return await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, numberOfCodes);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> SetTwoFactorAsync(string userName, bool enable)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        IdentityResult result = await userManager.SetTwoFactorEnabledAsync(user, enable);
        return result.Succeeded;
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<string?> GetAuthenticatorKeyAsync(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        await userManager.ResetAuthenticatorKeyAsync(user);
        return await userManager.GetAuthenticatorKeyAsync(user);
#else
        throw new NotImplementedException();
#endif
    }

    public async Task<bool> CheckTotpRecoveryCodes(string userName, string recoveryCode)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);
#if (UseIdentity)
        IdentityResult codeResult = await userManager.RedeemTwoFactorRecoveryCodeAsync(user, recoveryCode);

        return codeResult.Succeeded;
#else
        throw new NotImplementedException();
#endif
    }
}