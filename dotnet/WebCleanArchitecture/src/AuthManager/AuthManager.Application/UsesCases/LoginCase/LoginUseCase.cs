using AuthManager.Domain.BusinessObjects;
using AuthManager.Domain.ErrorsCode;
using AuthManager.Domain.InfraestructureContracts;
using Microsoft.Extensions.Logging;
using Shared;

namespace AuthManager.Application.UsesCases.LoginCase;

public class LoginUseCase(
    ILogger<LoginUserData> logger,
    IUserManager authManager,
    ITotpManager totpManager,
    IPasswordManager passwordManager,
    IEmailManager emailManager
) : ILoginUseCase
{
    public async Task<Result<LoginUserData>> ExecuteAsync(LoginCredentials request,
        CancellationToken cancellationToken = default)
    {
        UserData? user = await authManager.GetUserByUserNameAsync(request.UserName);
        if (user is null)
        {
            logger.LogWarning("Invalid username");
            return Result.Failure<LoginUserData>(new Error(UserManagerErrors.InvalidCredentialsCode,
                UserManagerErrors.InvalidCredentialsDesc));
        }


        bool result = await passwordManager.CheckPasswordAsync(user.UserName, request.Password);
        if (!result)
        {
            logger.LogWarning("Invalid password");
            return Result.Failure<LoginUserData>(new Error(UserManagerErrors.InvalidCredentialsCode,
                UserManagerErrors.InvalidCredentialsDesc));
        }

        bool isLocked = await authManager.IsLockedAsync(user.UserName);
        if (isLocked)
        {
            logger.LogWarning("User is locked");
            return Result.Failure<LoginUserData>(new(UserManagerErrors.LockedCode, UserManagerErrors.LockedDesc));
        }

        bool isEmailConfirm = await emailManager.CheckEmailConfirmAsync(user.UserName);
        if (!isEmailConfirm)
        {
            logger.LogWarning("User is locked");
            return Result.Failure<LoginUserData>(new(UserManagerErrors.EmailNotConfirmedCode,
                UserManagerErrors.EmailNotConfirmedDesc));
        }

        bool hasTotp = await totpManager.HasTwoFactorAuth(user.UserName);
        if (hasTotp && !string.IsNullOrWhiteSpace(request.TotpRecoveryCode))
        {
            bool isSucceded = await totpManager.CheckTotpRecoveryCodes(user.UserName, request.TotpRecoveryCode);
            if (!isSucceded)
            {
                logger.LogWarning("Invalid Recovery codes");
                return Result.Failure<LoginUserData>(new Error(UserManagerErrors.InvalidRecoveryCodesCode,
                    UserManagerErrors.InvalidRecoveryCodesDesc));
            }
        }
        else if (hasTotp && !await totpManager.VerifyTotpCodeAsync(user.UserName, request.TotpCode))
        {
            return Result.Failure<LoginUserData>(new Error(UserManagerErrors.TotpRequiredCode,
                UserManagerErrors.TotpRequiredDesc));
        }

        IList<string> roles = await authManager.GetUserRolesAsync(user.Id);

        return Result.Success(new LoginUserData(
            user.Id, user.UserName, user.Email, roles
        ));
    }
}