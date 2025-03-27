using AuthManager.API.Models;
using AuthManager.API.Models.JwtTokenModels;
using AuthManager.Application.Contracts;
using AuthManager.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using IJwtRepository = AuthManager.Application.Contracts.InfraestructureContracts.IJwtRepository;

namespace AuthManager.API.Controllers.TokenManagement;

[ApiController]
[Area(nameof(AuthManager))]
[Route("api/[area]/[controller]")]
public class RefreshTokenController(
    ILogger<RefreshTokenController> logger,
    IJwtRepository jwtRepository,
    IJwtTokenManagement jwtTokenManagement
) : Controller
{
    [HttpPut]
    public async Task<IActionResult> Post(JwtRefreshTokenRequest refreshTokenRequest)
    {
        IEnumerable<Claim> accessTokenClaims = jwtTokenManagement.ReadClaims(refreshTokenRequest.AccessToken);

        string refreshToken = accessTokenClaims.First(x => x.Type == ClaimsKey.RefreshToken).Value;

        DateTime expiration = await jwtRepository.GetTokenExpirationAsync(refreshToken);

        if (expiration < DateTime.UtcNow)
        {
            logger.LogWarning("Refresh token {RefreshTokenId} expired", refreshToken);
            return Unauthorized();
        }

        return Ok(new JwtResponse(jwtTokenManagement.Refresh(refreshTokenRequest.AccessToken)));
    }
}