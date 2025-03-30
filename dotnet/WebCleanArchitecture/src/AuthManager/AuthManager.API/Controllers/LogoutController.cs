using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.JWT;
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
        IEnumerable<Claim> claims = HttpContext.User.Claims;
        string userId = claims.First(x => x.Type == ClaimsKey.UserId).Value;
        string refreshTokenId = claims.First(x => x.Type == ClaimsKey.RefreshToken).Value;

        await jwtTokenManagement.RevokeRefreshTokenAsync(int.Parse(userId), refreshTokenId);

        return Ok();
    }
}