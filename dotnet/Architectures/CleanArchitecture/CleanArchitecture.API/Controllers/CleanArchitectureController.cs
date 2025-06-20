﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CleanArchitectureController(ILogger<CleanArchitectureController> logger) : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Get called");
        if (HttpContext.ToString() is null) { }

        return Ok();
    }
}
