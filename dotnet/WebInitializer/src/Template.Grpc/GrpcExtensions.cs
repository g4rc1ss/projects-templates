using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Template.Application;
using Template.Grpc.Services;

namespace Template.Grpc;

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
