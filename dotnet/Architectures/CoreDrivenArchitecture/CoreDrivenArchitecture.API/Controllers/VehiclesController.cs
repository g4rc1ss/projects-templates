using CoreDrivenArchitecture.API.Mappers;
using CoreDrivenArchitecture.API.Models;
using CoreDrivenArchitecture.DTOs.Vehicles;
using CoreDrivenArchitecture.UseCases.Vehicles;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace CoreDrivenArchitecture.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController(
    VehiclesUseCases vehicles
) : Controller
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        Result<VehicleDto> vehicle = await vehicles.GetVehicle.Execute(id);
        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateVehicleRequest request)
    {
        CreateVehicle createVehicle = request.ToUseCase();
        await vehicles.AddVehicle.Execute(createVehicle);
        return Ok();
    }
}