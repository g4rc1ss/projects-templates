using Microsoft.AspNetCore.Mvc;
#if (SqlDatabase)
using SimpleWeb.HostWebApi.UsesCases;
#endif

namespace SimpleWeb.HostWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimpleWebController(
#if (SqlDatabase)
    Example example,
#endif
    ILogger<SimpleWebController> logger
) : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Get called");
        if (HttpContext.ToString() is null) { }

        return Ok();
    }

    [HttpGet("Postgres")]
    public async Task<IActionResult> PostgresAsync()
    {
#if (UsePostgres)
        await example.PostgresAsync();
#endif
        return Ok();
    }


    [HttpGet("Sqlite")]
    public async Task<IActionResult> SqliteAsync()
    {
#if (UseSqlite)
        await example.SqliteAsync();
#endif
        return Ok();
    }
}