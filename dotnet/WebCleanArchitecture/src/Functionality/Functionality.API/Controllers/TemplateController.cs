using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Functionality.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController(ILogger<TemplateController> logger)
    : Controller
{
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok("Template");
    }
}
