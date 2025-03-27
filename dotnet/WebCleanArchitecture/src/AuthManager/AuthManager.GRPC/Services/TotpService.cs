using AuthManager.Application.Contracts;
using AuthManager.Application.Contracts.InfraestructureContracts;
using AuthManager.Domain;
using AuthManager.Domain.BusinessObjects;
using AuthManager.Domain.ErrorsCode;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthManager.GRPC.Services;

[Authorize]
public class TotpService(
    IJwtTokenManagement jwtTokenManagement,
    IUserManager userManager,
    ITotpManager totpManager
) : Totp.TotpBase
{
    public override async Task<ValidateTotpResponse> Validate(ValidateTotpRequest request, ServerCallContext context)
    {
        IEnumerable<Claim> claims = context.GetHttpContext().User.Claims;
        string? userId = claims.FirstOrDefault(x => x.Type == ClaimsKey.UserId)?.Value;

        UserData? user = await userManager.GetUserByIdAsync(userId);

        bool isVerify = await totpManager.VerifyTwoFactorTokenAsync(user.UserName, request.TotpCode);
        if (!isVerify)
        {
            throw new RpcException(new Status(StatusCode.NotFound, UserManagerErrors.TotpVerifyTotpCode));
        }

        IEnumerable<string>? recoveryCodes =
            await totpManager.GenerateRecoveryCodesAsync(user.UserName, 10) ?? [];

        await totpManager.SetTwoFactorAsync(user.UserName, true);

        return new ValidateTotpResponse
        {
            RecoveryCodes = string.Join(";", recoveryCodes),
        };
    }

    public override async Task<GenerateAuthKeyResponse> GenerateAuthenticator(GenerateAuthKeyRequest request,
        ServerCallContext context)
    {
        IEnumerable<Claim> claims = context.GetHttpContext().User.Claims;
        string? userId = claims.FirstOrDefault(x => x.Type == ClaimsKey.UserId)?.Value;

        UserData? user = await userManager.GetUserByIdAsync(userId);
        if (user is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, UserManagerErrors.InvalidUserCode));
        }

        string? key = await totpManager.GetAuthenticatorKeyAsync(user.UserName);

        return new GenerateAuthKeyResponse
        {
            Key = key,
        };
    }

    public override async Task<DisableTotpResponse> Disable(DisableTotpRequest request, ServerCallContext context)
    {
        IEnumerable<Claim> claims = context.GetHttpContext().User.Claims;
        string? userId = claims.FirstOrDefault(x => x.Type == ClaimsKey.UserId)?.Value;

        UserData? user = await userManager.GetUserByIdAsync(userId);
        await totpManager.SetTwoFactorAsync(user.UserName, false);
        return new DisableTotpResponse();
    }
}