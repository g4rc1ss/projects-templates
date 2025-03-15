using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Functionality.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FunctionalityController(ILogger<FunctionalityController> logger)
    : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        if (HttpContext.ToString() is null)
        {
        }

        return Ok();
    }
}