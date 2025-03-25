using AuthManager.API.Mapper;
using AuthManager.API.Models.SignUpModels;
using AuthManager.Application.UsesCases.CreateUserCase;
using Infraestructure.Events;
using Infraestructure.Events.SignUpEmailEvent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;

namespace AuthManager.API.Controllers;

[ApiController]
[Area(nameof(AuthManager))]
[Route("api/[area]/[controller]")]
public class SignUpController(
    ILogger<SignUpController> logger,
    ICreateUserUseCase createUser,
    IEventNotificator eventNotificator
) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Post(SignUpRequest request, CancellationToken cancellationToken)
    {
        CreateUserData userData = request.ToUserData();

        Result<string> result = await createUser.ExecuteAsync(userData, cancellationToken);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        string? url = Url.Action("ConfirmEmail", "ConfirmEmail", new
        {
            userName = request.UserName,
            Token = result.Data,
        }, Request.Scheme);

        await eventNotificator.PublishAsync(new ConfirmEmailRequest(request.Email, url), cancellationToken);

        return Ok();
    }
}