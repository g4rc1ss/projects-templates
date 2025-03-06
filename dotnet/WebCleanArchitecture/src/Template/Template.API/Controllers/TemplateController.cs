using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Template.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplatePruebaController(ILogger<TemplatePruebaController> logger)
    : Controller
{
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok("Template");
    }
}
