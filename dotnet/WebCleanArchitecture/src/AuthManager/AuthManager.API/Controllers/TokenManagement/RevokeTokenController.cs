using AuthManager.API.Models.JwtTokenModels;
using AuthManager.Domain;
using AuthManager.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        string userId = jwtTokenManagement.ReadClaims().First(x => x.Type == ClaimsKey.UserId).Value;
        string token = revokeTokenRequest.TokenId;

        await jwtTokenManagement.RevokeRefreshTokenAsync(int.Parse(userId), token);

        return Ok();
    }
}