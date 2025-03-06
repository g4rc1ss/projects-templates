using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Template.API.Controllers;

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
