using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompletedWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController(ILogger<TemplateController> logger) : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Get called");
        if (HttpContext.ToString() is null) { }

        return Ok();
    }
}
