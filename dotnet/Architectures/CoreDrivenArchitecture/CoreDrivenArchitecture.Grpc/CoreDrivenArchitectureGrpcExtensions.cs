using CoreDrivenArchitecture.Data;
using CoreDrivenArchitecture.Grpc.Services;
using CoreDrivenArchitecture.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CoreDrivenArchitecture.Grpc;

public static class CoreDrivenArchitectureGrpcExtensions
{
    public static void InitCoreDrivenArchitectureGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddUseCases();
        builder.Services.AddData();
    }

    public static void MapCleanArchitectureGrpcServices(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<VehicleService>();
    }
}