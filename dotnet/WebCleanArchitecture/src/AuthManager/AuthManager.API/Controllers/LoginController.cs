using AuthManager.API.Mapper;
using AuthManager.API.Models;
using AuthManager.API.Models.LoginModels;
using AuthManager.Application.UsesCases.LoginCase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using System.Security.Claims;

namespace AuthManager.API.Controllers;

[ApiController]
[Area(nameof(AuthManager))]
[Route("api/[area]/[controller]")]
public class LoginController(
    ILogger<LoginController> logger,
    ILoginUseCase loginUseCase,
    IJwtTokenManagement jwtTokenManagement
) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Post(LoginRequest request)
    {
        LoginCredentials loginCredentials = LoginMapper.RequestToLoginCredentials(request);

        Result<LoginUserData> result = await loginUseCase.ExecuteAsync(loginCredentials);
        if (!result.IsSuccess)
        {
            return Unauthorized(result);
        }

        string refreshToken = await jwtTokenManagement.CreateRefreshTokenAsync(result.Data.UserId);

        IEnumerable<Claim> claims = result?.Data?.Roles
            ?.Select(role => new Claim(ClaimTypes.Role, role)) ?? [];

        JwtData jwtData = new(
            result.Data.UserId.ToString(),
            result.Data.UserName,
            result.Data.Email,
            refreshToken,
            claims
        );

        return Ok(new JwtResponse(jwtTokenManagement.Create(jwtData)));
    }
}