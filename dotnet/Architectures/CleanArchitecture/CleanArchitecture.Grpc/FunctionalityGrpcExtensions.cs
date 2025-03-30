using CleanArchitecture.Application;
using CleanArchitecture.Grpc.Services;
using CleanArchitecture.Infraestructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CleanArchitecture.Grpc;

public static class FunctionalityGrpcExtensions
{
    public static void InitFunctionalityGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
        builder.AddDataAccessService();
    }

    public static void MapFunctionalityGrpcServices(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}