using Microsoft.AspNetCore.Mvc;
#if (SqlDatabase || NoSqlDatabase)
using SimpleWeb.HostWebApi.UsesCases;
#endif

namespace SimpleWeb.HostWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController(
#if (SqlDatabase || NoSqlDatabase)
    Example example,
#endif
    ILogger<TemplateController> logger) : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Get called");
        if (HttpContext.ToString() is null) { }

        return Ok();
    }

#if (UsePostgres)
    [HttpGet("Postgres")]
    public async Task<IActionResult> PostgresAsync()
    {
        await example.PostgresAsync();
        return Ok();
    }
#endif

#if (UseSqlite)
    [HttpGet("Sqlite")]
    public async Task<IActionResult> SqliteAsync()
    {
        await example.SqliteAsync();
        return Ok();
    }
#endif
#if (UseMongodb)
    [HttpGet("Mongodb")]
    public async Task<IActionResult> MongodbAsync()
    {
        await example.MongodbAsync();
        return Ok();
    }
#endif
#if (UseSqlite)
    [HttpGet("Litedb")]
    public async Task<IActionResult> LitedbAsync()
    {
        await example.LitedbAsync();
        return Ok();
    }
#endif
}
