using AuthManager.Application.Contracts.InfraestructureContracts;
using AuthManager.Domain.BusinessObjects;
using AuthManager.Domain.ErrorsCode;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Shared.JWT;
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
            throw new RpcException(new Status(StatusCode.NotFound, UserManagerErrors.InvalidCredentialsCode));
        }

        bool isChanged = await passwordManager.ChangePasswordAsync(userData.UserName,
            request.OldPassword,
            request.NewPassword);
        if (!isChanged)
        {
            throw new RpcException(new Status(StatusCode.NotFound, PasswordManagerErrors.PasswordChangeErrorCode));
        }

        return new ChangePasswordResponse();
    }

    public override async Task<ConfirmPasswordResponse> Confirm(ConfirmPasswordRequest request,
        ServerCallContext context)
    {
        bool isConfirmed =
            await passwordManager.ConfirmPasswordTokenAsync(request.Username, request.Token, request.Password);

        return isConfirmed
            ? new ConfirmPasswordResponse()
            : throw new RpcException(new Status(StatusCode.NotFound, PasswordManagerErrors.PasswordConfirmErrorCode));
    }

    public override async Task<ResetPasswordResponse> Reset(ResetPasswordRequest request, ServerCallContext context)
    {
        UserData? userData = await userManager.GetUserByUserNameAsync(request.Username);
        if (userData is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, UserManagerErrors.InvalidCredentialsCode));
        }

        string passwordToken = await passwordManager.GetResetTokenAsync(userData.UserName);
        // string? url = Url.Action("ConfirmEmail", "ConfirmEmail", new
        // {
        //     userName = userName,
        //     Token = passwordToken,
        // }, Request.Scheme);

        return new ResetPasswordResponse();
    }
}