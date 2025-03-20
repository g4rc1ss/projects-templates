using AuthManager.API.Models.PasswordModels;
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

[ApiController]
[Area(nameof(AuthManager))]
[Route("api/[area]/[controller]")]
public class PasswordController(
    IJwtTokenManagement jwtTokenManagement,
    IUserManager userManager,
    IPasswordManager passwordManager
) : Controller
{
    [Authorize]
    [HttpPost("change")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        IEnumerable<Claim> claims = jwtTokenManagement.ReadClaims();
        string? userId = claims.FirstOrDefault(x => x.Type == ClaimsKey.UserId)?.Value;

        UserData? userData = await userManager.GetUserByIdAsync(userId);
        if (userData is null)
        {
            return NotFound(Result.Failure(new Error(UserManagerErrors.InvalidCredentialsCode,
                UserManagerErrors.InvalidCredentialsDesc)));
        }

        bool isChanged = await passwordManager.ChangePasswordAsync(userData.UserName,
            changePasswordRequest.OldPassword,
            changePasswordRequest.NewPassword);
        if (!isChanged)
        {
            return NotFound(Result.Failure(new Error(PasswordManagerErrors.PasswordChangeErrorCode,
                PasswordManagerErrors.PasswordChangeErrorDesc)));
        }

        return Ok();
    }

    [HttpPost("reset/{userName}")]
    public async Task<IActionResult> ResetPassword(string userName)
    {
        UserData? userData = await userManager.GetUserByUserNameAsync(userName);
        if (userData is null)
        {
            return NotFound(Result.Failure(new Error(UserManagerErrors.InvalidCredentialsCode,
                UserManagerErrors.InvalidCredentialsDesc)));
        }

        string passwordToken = await passwordManager.GetResetTokenAsync(userData.UserName);
        string? url = Url.Action("ConfirmEmail", "ConfirmEmail", new
        {
            userName = userName,
            Token = passwordToken,
        }, Request.Scheme);

        return Ok();
    }

    [HttpPost("confirm-password/{userName}")]
    public async Task<IActionResult> ConfirmPassword(string userName, ConfirmPassword password)
    {
        bool isConfirmed = await passwordManager.ConfirmPasswordTokenAsync(userName, password.Token, password.Password);

        return isConfirmed ? Ok() : NotFound();
    }
}