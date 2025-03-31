using CoreDrivenArchitecture.Data.Entities;
using CoreDrivenArchitecture.Data.Repositories;
using CoreDrivenArchitecture.DTOs.Vehicles;
using CoreDrivenArchitecture.UseCases.Mappers;
using ROP;

namespace CoreDrivenArchitecture.UseCases.Vehicles;

public class AddVehicle(
    IDatabaseRepository databaseRepository
)
{
    public async Task<Result<VehicleDto>> Execute(CreateVehicle request)
    {
        VehicleEntity vehicleEntity = await databaseRepository.AddVehicle(request);
        VehicleDto dto = vehicleEntity.ToDto();
        // await eventNotificator.Notify(dto);
        return dto;
    }
}