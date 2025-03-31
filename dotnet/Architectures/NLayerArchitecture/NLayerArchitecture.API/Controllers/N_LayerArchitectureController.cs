﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NLayerArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NLayerArchitectureController(
    ILogger<NLayerArchitectureController> logger
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