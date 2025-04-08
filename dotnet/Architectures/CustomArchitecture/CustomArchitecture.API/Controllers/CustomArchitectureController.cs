﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomArchitectureController(
    ILogger<CustomArchitectureController> logger
) : Controller
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