using CompletedWeb.Application;
using CompletedWeb.Grpc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CompletedWeb.Grpc;

public static class GrpcExtensions
{
    public static void InitWebGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
    }

    public static void MapWebGrpc(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}
