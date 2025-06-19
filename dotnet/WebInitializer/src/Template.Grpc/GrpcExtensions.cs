using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Template.Application;
using Template.Grpc.Services;

namespace Template.Grpc;

public static class GrpcExtensions
{
    public static void InitTemplateGrpc(this WebApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
    }

    public static void MapTemplateGrpc(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}
