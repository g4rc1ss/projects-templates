using CustomArchitecture.Application;
using CustomArchitecture.Grpc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CustomArchitecture.Grpc;

public static class GrpcExtensions
{
    public static void InitCustomArchitectureGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
    }

    public static void MapCustomArchitectureGrpc(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}
