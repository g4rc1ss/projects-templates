using AuthManager.Application.UsesCases.LoginCase;
using AuthManager.GRPC.Mapper;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.JWT;
using System.Security.Claims;

namespace AuthManager.GRPC.Services;

public class LoginService(
    ILogger<LoginService> logger,
    ILoginUseCase loginUseCase,
    IJwtTokenManagement jwtTokenManagement
) : Login.LoginBase
{
    public override async Task<LoginResponse> DoLogin(LoginRequest request, ServerCallContext context)
    {
        LoginCredentials loginCredentials = LoginMapper.RequestToLoginCredentials(request);

        Result<LoginUserData> result = await loginUseCase.ExecuteAsync(loginCredentials);
        if (!result.IsSuccess)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, result.Error.Code));
        }

        string refreshToken = await jwtTokenManagement.CreateRefreshTokenAsync(result.Data.UserId);

        IEnumerable<Claim> claims = result?.Data?.Roles
            ?.Select(role => new Claim(ClaimTypes.Role, role)) ?? [];

        JwtData jwtData = new(
            result.Data.UserId.ToString(),
            result.Data.UserName,
            result.Data.Email,
            refreshToken,
            claims
        );

        return new LoginResponse
        {
            AccessToken = jwtTokenManagement.Create(jwtData)
        };
    }
}