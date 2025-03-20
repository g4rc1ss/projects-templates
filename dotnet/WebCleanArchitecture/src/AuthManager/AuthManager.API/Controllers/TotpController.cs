using AuthManager.Domain;
using AuthManager.Domain.BusinessObjects;
using AuthManager.Domain.ErrorsCode;
using AuthManager.Domain.InfraestructureContracts;
using AuthManager.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;

namespace AuthManager.API.Controllers;

[Authorize]
[ApiController]
[Area(nameof(AuthManager))]
[Route("api/[area]/[controller]")]
public class TotpController(
    IJwtTokenManagement jwtTokenManagement,
    IUserManager userManager,
    ITotpManager totpManager
) : Controller
{
    [HttpPost("validate/{totpCode}")]
    public async Task<IActionResult> Activate(string totpCode)
    {
        IEnumerable<Claim> claims = jwtTokenManagement.ReadClaims();
        string? userId = claims.FirstOrDefault(x => x.Type == ClaimsKey.UserId)?.Value;

        UserData? user = await userManager.GetUserByIdAsync(userId);

        bool isVerify = await totpManager.VerifyTwoFactorTokenAsync(user.UserName, totpCode);
        if (!isVerify)
        {
            return Unauthorized(Result.Failure(new(UserManagerErrors.TotpVerifyTotpCode,
                UserManagerErrors.TotpVerifyTotpDesc)));
        }

        IEnumerable<string>? recoveryCodes =
            await totpManager.GenerateRecoveryCodesAsync(user.UserName, 10) ?? [];

        await totpManager.SetTwoFactorAsync(user.UserName, true);

        return Ok(string.Join(";", recoveryCodes));
    }

    [HttpGet("generate-authenticator-key")]
    public async Task<IActionResult> GenerateAuthenticatorKey()
    {
        IEnumerable<Claim> claims = jwtTokenManagement.ReadClaims();
        string? userId = claims.FirstOrDefault(x => x.Type == ClaimsKey.UserId)?.Value;

        UserData? user = await userManager.GetUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }

        string? key = await totpManager.GetAuthenticatorKeyAsync(user.UserName);

        return Ok(key);
    }

    [HttpPost("deactivate")]
    public async Task<IActionResult> Deactivate()
    {
        IEnumerable<Claim> claims = jwtTokenManagement.ReadClaims();
        string? userId = claims.FirstOrDefault(x => x.Type == ClaimsKey.UserId)?.Value;

        UserData? user = await userManager.GetUserByIdAsync(userId);
        await totpManager.SetTwoFactorAsync(user.UserName, false);
        return Ok();
    }
}