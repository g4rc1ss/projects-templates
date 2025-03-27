using AuthManager.Application.Contracts;
using AuthManager.Application.Contracts.InfraestructureContracts;
using AuthManager.Domain;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AuthManager.GRPC.Services.TokenManagement;

public class RefreshTokenService(
    ILogger<RefreshTokenService> logger,
    IJwtRepository jwtRepository,
    IJwtTokenManagement jwtTokenManagement
) : RefreshToken.RefreshTokenBase
{
    public override async Task<JwtRefreshTokenResponse> Refresh(JwtRefreshTokenRequest request,
        ServerCallContext context)
    {
        IEnumerable<Claim> accessTokenClaims = jwtTokenManagement.ReadClaims(request.AccessToken);

        string refreshToken = accessTokenClaims.First(x => x.Type == ClaimsKey.RefreshToken).Value;

        DateTime expiration = await jwtRepository.GetTokenExpirationAsync(refreshToken);

        if (expiration < DateTime.UtcNow)
        {
            logger.LogWarning("Refresh token {RefreshTokenId} expired", refreshToken);
            throw new RpcException(new Status(StatusCode.PermissionDenied, "Access token expired"));
        }

        return new JwtRefreshTokenResponse
        {
            AccessToken = jwtTokenManagement.Refresh(request.AccessToken),
        };
    }
}