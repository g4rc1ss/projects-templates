using Microsoft.AspNetCore.Mvc;

namespace SimpleWeb.HostWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimpleWebController(ILogger<SimpleWebController> logger) : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Get called");
        if (HttpContext.ToString() is null) { }

        return Ok();
    }
}
