using AuthManager.Domain.BusinessObjects;
using AuthManager.Domain.InfraestructureContracts;
using Microsoft.AspNetCore.Mvc;

namespace AuthManager.API.Controllers;

[ApiController]
[Area(nameof(AuthManager))]
[Route("api/[area]/[controller]")]
public class ConfirmEmailController(
    IUserManager userManager,
    IEmailManager emailManager
) : Controller
{
    [HttpGet("{userName}")]
    public async Task<IActionResult> ConfirmEmailAsync(string userName, string token)
    {
        UserData? user = await userManager.GetUserByUserNameAsync(userName);
        if (user is null)
        {
            return NotFound();
        }

        bool confirm = await emailManager.ConfirmEmailAsync(user.UserName, token);

        return Ok(confirm);
    }
}