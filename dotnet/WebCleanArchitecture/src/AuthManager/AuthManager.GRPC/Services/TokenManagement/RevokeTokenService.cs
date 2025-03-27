using AuthManager.Application.Contracts;
using AuthManager.Domain;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AuthManager.GRPC.Services.TokenManagement;

[Authorize]
public class RevokeTokenService(
    ILogger<RevokeTokenService> logger,
    IJwtTokenManagement jwtTokenManagement
) : RevokeToken.RevokeTokenBase
{
    public override async Task<RevokeTokenResponse> Revoke(RevokeTokenRequest request, ServerCallContext context)
    {
        string userId = context.GetHttpContext().User.Claims.First(x => x.Type == ClaimsKey.UserId).Value;
        string token = request.TokenId;

        await jwtTokenManagement.RevokeRefreshTokenAsync(int.Parse(userId), token);

        return new RevokeTokenResponse();
    }
}