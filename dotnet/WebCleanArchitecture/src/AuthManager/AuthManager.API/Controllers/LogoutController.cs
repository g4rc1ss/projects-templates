using AuthManager.Domain;
using AuthManager.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AuthManager.API.Controllers;

[Authorize]
[Area(nameof(AuthManager))]
[ApiController]
[Route("api/[area]/[controller]")]
public class LogoutController(
    ILogger<LogoutController> logger,
    IJwtTokenManagement jwtTokenManagement
) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        IEnumerable<Claim> claims = jwtTokenManagement.ReadClaims();
        string userId = claims.First(x => x.Type == ClaimsKey.UserId).Value;
        string refreshTokenId = claims.First(x => x.Type == ClaimsKey.RefreshToken).Value;

        await jwtTokenManagement.RevokeRefreshTokenAsync(int.Parse(userId), refreshTokenId);

        return Ok();
    }
}