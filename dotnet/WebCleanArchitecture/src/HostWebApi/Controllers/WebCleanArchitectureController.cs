using Microsoft.AspNetCore.Mvc;

namespace HostWebApi.Controllers;

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
