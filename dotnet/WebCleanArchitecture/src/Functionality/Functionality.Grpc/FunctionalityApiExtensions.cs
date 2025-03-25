using Functionality.Application;
using Functionality.Grpc.Services;
using Functionality.Infraestructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Functionality.Grpc;

public static class FunctionalityApiExtensions
{
    public static void InitFunctionality(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(FunctionalityApiExtensions).Assembly);

        builder.Services.AddBusinessServices();
        builder.Services.AddDataAccessService(builder.Configuration);
    }

    public static void MapFunctionalityGrpcServices(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}