using AuthManager.Domain.InfraestructureContracts;
using Infraestructure.Database.Entities;
using Infraestructure.Database.Repository;
using Microsoft.AspNetCore.Identity;

namespace AuthManager.Infraestructure.Repositories.Identity;

public class TotpManager(
    UserManager<UserEntity> userManager,
    IUserInfo userInfo
) : ITotpManager
{
    public async Task<bool> HasTwoFactorAuth(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        return await userManager.GetTwoFactorEnabledAsync(user);
    }

    public async Task<bool> VerifyTotpCodeAsync(string userName, string? totpCode)
    {
        if (string.IsNullOrEmpty(totpCode))
        {
            return false;
        }

        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        return await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider,
            totpCode);
    }

    public async Task<bool> VerifyTwoFactorTokenAsync(string userName, string totpCode)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        return await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, totpCode);
    }

    public async Task<IEnumerable<string>?> GenerateRecoveryCodesAsync(string userName, int numberOfCodes)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        return await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, numberOfCodes);
    }

    public async Task<bool> SetTwoFactorAsync(string userName, bool enable)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        IdentityResult result = await userManager.SetTwoFactorEnabledAsync(user, enable);
        return result.Succeeded;
    }

    public async Task<string?> GetAuthenticatorKeyAsync(string userName)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        await userManager.ResetAuthenticatorKeyAsync(user);
        return await userManager.GetAuthenticatorKeyAsync(user);
    }

    public async Task<bool> CheckTotpRecoveryCodes(string userName, string recoveryCode)
    {
        UserEntity? user = await userInfo.GetUserByUserNameAsync(userName);

        IdentityResult codeResult = await userManager.RedeemTwoFactorRecoveryCodeAsync(user, recoveryCode);

        return codeResult.Succeeded;
    }
}