using CoreDrivenArchitecture.Data.Entities;
using CoreDrivenArchitecture.Data.Repositories;
using CoreDrivenArchitecture.DTOs.Vehicles;
using CoreDrivenArchitecture.UseCases.Mappers;

namespace CoreDrivenArchitecture.UseCases.Vehicles;

public class GetVehicle (IDatabaseRepository databaseRepository)
{
    public async Task<VehicleDto> Execute(int id)
    {
        VehicleEntity vehicleEntity =  await databaseRepository.GetVehicle(id);

        return vehicleEntity.ToDto();
    }
}