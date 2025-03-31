using AuthManager.API.Models.JwtTokenModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.JWT;
using IJwtRepository = Shared.JWT.IJwtRepository;
using JwtTokenData = Shared.JWT.JwtTokenData;

namespace AuthManager.API.Controllers.TokenManagement;

[Authorize]
[ApiController]
[Area(nameof(AuthManager))]
[Route("api/[area]/[controller]")]
public class TokenListController(
    ILogger<TokenListController> logger,
    IJwtTokenManagement jwtTokenManagement,
    IJwtRepository jwtTokenRepository
) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        IEnumerable<JwtTokenData> tokens = await jwtTokenRepository.GetAllTokensByUserId(1);

        JwtTokenListResponse returnList = new(tokens.Select(x =>
            new TokenInformation(x.TokenId, x.Expiration)));

        return Ok(returnList);
    }
}