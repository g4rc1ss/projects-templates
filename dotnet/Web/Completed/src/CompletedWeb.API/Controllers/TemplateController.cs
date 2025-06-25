using CompletedWeb.Application.UsesCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompletedWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController(
    ILogger<TemplateController> logger,
    IExample example
) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Get called");
        if (HttpContext.ToString() is null) { }

        await example.ExecuteAsync(cancellationToken);

        return Ok();
    }
}