using CoreDrivenArchitecture.Data.Entities;
using CoreDrivenArchitecture.DTOs.Vehicles;

namespace CoreDrivenArchitecture.Data.Repositories;

public interface IDatabaseRepository
{
    Task<VehicleEntity> AddVehicle(CreateVehicle vehicle);
    Task<VehicleEntity> GetVehicle(int id);
}

public class DatabaseRepository : IDatabaseRepository
{
    public Task<VehicleEntity> AddVehicle(CreateVehicle vehicle)
    {
        throw new NotImplementedException();
    }

    public Task<VehicleEntity> GetVehicle(int id)
    {
        throw new NotImplementedException();
    }
}