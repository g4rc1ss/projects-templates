using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
#if (UseIdentity)
using Infraestructure.Auth.IdentityAuth;
#endif
#if (UseJwt)
using System.Security.Claims;
using Infraestructure.Auth.JwtManager;
using Infraestructure.Auth.JwtManager.JwtTokenModels;
using Infraestructure.Auth.JwtManager.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
#endif

namespace Infraestructure.Auth;

public static class AuthEndpointsExtensions
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder authGroup = endpoints.MapGroup("api/auth");
#if (UseIdentity)
        authGroup.MapIdentityApi<IdentityUserEntity>();
#endif
#if (UseJwt)
        authGroup.MapJwt();
#endif
    }

#if (UseJwt)
    private static void MapJwt(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("refresh-token", RefreshTokenAsync).Produces<JwtResponse>();
        endpoints.MapDelete("revoke-token", RevokeTokenAsync).Produces<JwtResponse>();
        endpoints.MapGet("token-list", TokenListAsync).Produces<JwtResponse>();
    }

    private static async Task<IResult> RefreshTokenAsync(
        [AsParameters] JwtRefreshTokenRequest refreshTokenRequest,
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] IJwtRepository jwtRepository,
        [FromServices] IJwtTokenManagement jwtTokenManagement
    )
    {
        ILogger logger = loggerFactory.CreateLogger("JwtTokenManagement");

        IEnumerable<Claim> accessTokenClaims = jwtTokenManagement.ReadClaims(
            refreshTokenRequest.AccessToken
        );

        string refreshToken = accessTokenClaims.First(x => x.Type == ClaimsKey.REFRESH_TOKEN).Value;

        DateTime expiration = await jwtRepository.GetTokenExpirationAsync(refreshToken);

        if (expiration < DateTime.UtcNow)
        {
            logger.LogWarning("Refresh token {RefreshTokenId} expired", refreshToken);
            return Results.Unauthorized();
        }

        return Results.Ok(
            new JwtResponse(jwtTokenManagement.Refresh(refreshTokenRequest.AccessToken))
        );
    }

    private static async Task<IResult> RevokeTokenAsync(
        [AsParameters] JwtRevokeTokenRequest revokeTokenRequest,
        [FromServices] IJwtTokenManagement jwtTokenManagement
    )
    {
        string userId = jwtTokenManagement
            .ReadClaims()
            .First(x => x.Type == ClaimsKey.USER_ID)
            .Value;
        string token = revokeTokenRequest.TokenId;

        await jwtTokenManagement.RevokeRefreshTokenAsync(int.Parse(userId), token);

        return Results.Ok();
    }

    private static async Task<IResult> TokenListAsync([FromServices] IJwtRepository jwtRepository)
    {
        IEnumerable<JwtTokenData> tokens = await jwtRepository.GetAllTokensByUserId(1);

        JwtTokenListResponse returnList = new(
            tokens.Select(x => new TokenInformation(x.TokenId, x.Expiration))
        );

        return Results.Ok(returnList);
    }
#endif
}
