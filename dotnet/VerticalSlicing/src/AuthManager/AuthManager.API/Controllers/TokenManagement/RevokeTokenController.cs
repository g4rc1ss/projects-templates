using AuthManager.API.Models.JwtTokenModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.JWT;

namespace AuthManager.API.Controllers.TokenManagement;

[Authorize]
[ApiController]
[Area(nameof(AuthManager))]
[Route("api/[area]/[controller]")]
public class RevokeTokenController(
    ILogger<RevokeTokenController> logger,
    IJwtTokenManagement jwtTokenManagement
) : Controller
{
    [HttpDelete]
    public async Task<IActionResult> PostAsync(JwtRevokeTokenRequest revokeTokenRequest)
    {
        string userId = HttpContext.User.Claims.First(x => x.Type == ClaimsKey.UserId).Value;
        string token = revokeTokenRequest.TokenId;

        await jwtTokenManagement.RevokeRefreshTokenAsync(int.Parse(userId), token);

        return Ok();
    }
}