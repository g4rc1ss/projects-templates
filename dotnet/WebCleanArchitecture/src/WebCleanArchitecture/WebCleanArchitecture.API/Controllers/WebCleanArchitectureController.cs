using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebCleanArchitecture.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WebCleanArchitectureController(ILogger<WebCleanArchitectureController> logger)
    : Controller
{
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok("WebCleanArchitecture");
    }
}
