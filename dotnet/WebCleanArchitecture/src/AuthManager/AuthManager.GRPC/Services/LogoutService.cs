using AuthManager.Application.Contracts;
using AuthManager.Domain;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AuthManager.GRPC.Services;

[Authorize]
public class LogoutService(
    ILogger<LogoutService> logger,
    IJwtTokenManagement jwtTokenManagement
) : Logout.LogoutBase
{
    public override async Task<LogoutResponse> DoLogout(LogoutRequest request, ServerCallContext context)
    {
        IEnumerable<Claim> claims = context.GetHttpContext().User.Claims;
        string userId = claims.First(x => x.Type == ClaimsKey.UserId).Value;
        string refreshTokenId = claims.First(x => x.Type == ClaimsKey.RefreshToken).Value;

        await jwtTokenManagement.RevokeRefreshTokenAsync(int.Parse(userId), refreshTokenId);

        return new LogoutResponse();
    }
}