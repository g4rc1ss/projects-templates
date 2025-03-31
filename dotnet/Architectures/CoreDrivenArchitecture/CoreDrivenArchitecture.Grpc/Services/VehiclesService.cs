using CoreDrivenArchitecture.DTOs.Vehicles;
using CoreDrivenArchitecture.Grpc.Mappers;
using CoreDrivenArchitecture.UseCases.Vehicles;
using Grpc.Core;
using ROP;

namespace CoreDrivenArchitecture.Grpc.Services;

public class VehiclesService(
    VehiclesUseCases vehicles
) : Vehicle.VehicleBase
{
    public override async Task<CreateVehicleResponse> Create(CreateVehicleRequest request, ServerCallContext context)
    {
        CreateVehicle createVehicle = request.ToUseCase();
        await vehicles.AddVehicle.Execute(createVehicle);
        return new CreateVehicleResponse();
    }

    public override async Task<GetVehicleResponse> Get(GetVehicleRequest request, ServerCallContext context)
    {
        Result<VehicleDto> vehicle = await vehicles.GetVehicle.Execute(request.Id);
        if (!vehicle.Success)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Vehicle with id: {request.Id} not found"));
        }

        return vehicle.Value.ToHostResponse();
    }
}