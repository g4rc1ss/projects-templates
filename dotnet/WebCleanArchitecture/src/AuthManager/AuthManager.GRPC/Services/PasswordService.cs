using AuthManager.Application.Contracts;
using AuthManager.Application.Contracts.InfraestructureContracts;
using AuthManager.Domain;
using AuthManager.Domain.BusinessObjects;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthManager.GRPC.Services;

[Authorize]
public class PasswordService(
    IJwtTokenManagement jwtTokenManagement,
    IUserManager userManager,
    IPasswordManager passwordManager
) : Password.PasswordBase
{
    public override async Task<ChangePasswordResponse> Change(ChangePasswordRequest request, ServerCallContext context)
    {
        IEnumerable<Claim> claims = context.GetHttpContext().User.Claims;
        string? userId = claims.FirstOrDefault(x => x.Type == ClaimsKey.UserId)?.Value;

        UserData? userData = await userManager.GetUserByIdAsync(userId);
        if (userData is null)
        {
            // TODO: Devolver codigo de error

            // return NotFound(Result.Failure(new Error(UserManagerErrors.InvalidCredentialsCode,
            //     UserManagerErrors.InvalidCredentialsDesc)));
        }

        bool isChanged = await passwordManager.ChangePasswordAsync(userData.UserName,
            request.OldPassword,
            request.NewPassword);
        if (!isChanged)
        {
            // TODO: Devolver codigo de error

            // return NotFound(Result.Failure(new Error(PasswordManagerErrors.PasswordChangeErrorCode,
            //     PasswordManagerErrors.PasswordChangeErrorDesc)));
        }

        // TODO: Devolver success
        return new ChangePasswordResponse();
    }

    public override Task<ConfirmPasswordResponse> Confirm(ConfirmPasswordRequest request, ServerCallContext context)
    {
        return base.Confirm(request, context);
    }

    public override Task<ResetPasswordResponse> Reset(ResetPasswordRequest request, ServerCallContext context)
    {
        return base.Reset(request, context);
    }
}