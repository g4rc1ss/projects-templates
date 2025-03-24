using AuthManager.API.Models.JwtTokenModels;
using AuthManager.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IJwtRepository = AuthManager.Application.Contracts.InfraestructureContracts.IJwtRepository;
using JwtTokenData = AuthManager.Application.Contracts.InfraestructureContracts.JwtTokenData;

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