using Microsoft.AspNetCore.Mvc;
using SimpleWeb.HostWebApi.UsesCases;

namespace SimpleWeb.HostWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController(Example example, ILogger<TemplateController> logger) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        logger.LogInformation("Get called");
        if (HttpContext.ToString() is null) { }

        await example.ExecuteAsync();

        return Ok();
    }
}
