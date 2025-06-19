using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using NLayerArchitecture.Application;
using NLayerArchitecture.Grpc.Services;
using NLayerArchitecture.Infraestructure;

namespace NLayerArchitecture.Grpc;

public static class GrpcExtensions
{
    public static void InitNLayerArchitecture(this WebApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
        builder.AddDataAccessService();
    }

    public static void MapNLayerArchitecture(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}
