using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using MvcArchitecture.Grpc.Services;

namespace MvcArchitecture.Grpc;

public static class GrpcExtensions
{
    public static void InitMvcArchitectureGrpc(this WebApplicationBuilder builder) { }

    public static void MapMvcArchitectureGrpc(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}
