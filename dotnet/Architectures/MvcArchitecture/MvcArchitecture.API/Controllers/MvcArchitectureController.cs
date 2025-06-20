using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MvcArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MvcArchitectureController(ILogger<MvcArchitectureController> logger) : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Get called");
        if (HttpContext.ToString() is null) { }

        return Ok();
    }
}
