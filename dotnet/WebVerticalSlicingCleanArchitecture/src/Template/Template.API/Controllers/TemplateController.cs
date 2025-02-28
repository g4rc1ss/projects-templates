using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Template.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TemplateController(ILogger<TemplateController> logger)
    : Controller
{
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok("Template");
    }
}
