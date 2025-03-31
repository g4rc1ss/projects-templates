using CoreDrivenArchitecture.DTOs.Vehicles;
using Riok.Mapperly.Abstractions;

namespace CoreDrivenArchitecture.API.Mappers;

[Mapper]
public static partial class VehiclesMapper
{
    public static partial CreateVehicle ToUseCase(this CreateVehicleRequest request);
}