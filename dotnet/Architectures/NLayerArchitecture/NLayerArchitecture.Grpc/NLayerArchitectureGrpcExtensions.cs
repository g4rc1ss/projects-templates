using NLayerArchitecture.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using NLayerArchitecture.Grpc.Services;
using NLayerArchitecture.Infraestructure;

namespace NLayerArchitecture.Grpc;

public static class NLayerArchitectureGrpcExtensions
{
    public static void NLayerArchitectureGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
        builder.AddDataAccessService();
    }

    public static void MapNLayerArchitectureGrpcServices(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}