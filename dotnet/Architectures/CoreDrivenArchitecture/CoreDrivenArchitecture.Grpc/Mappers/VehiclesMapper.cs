using CoreDrivenArchitecture.DTOs.Vehicles;
using Riok.Mapperly.Abstractions;

namespace CoreDrivenArchitecture.Grpc.Mappers;

[Mapper]
public static partial class VehiclesMapper
{
    public static partial CreateVehicle ToUseCase(this CreateVehicleRequest request);

    public static partial GetVehicleResponse ToHostResponse(this VehicleDto response);
}