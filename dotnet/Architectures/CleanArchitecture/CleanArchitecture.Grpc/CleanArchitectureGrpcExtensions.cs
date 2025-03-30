using CleanArchitecture.Application;
using CleanArchitecture.Grpc.Services;
using CleanArchitecture.Infraestructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CleanArchitecture.Grpc;

public static class CleanArchitectureGrpcExtensions
{
    public static void InitCleanArchitectureGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
        builder.AddDataAccessService();
    }

    public static void MapCleanArchitectureGrpcServices(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}