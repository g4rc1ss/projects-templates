using Functionality.Application;
using Functionality.Grpc.Services;
using Functionality.Infraestructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
#if (UseApi || UseGrpc)
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Functionality.Grpc;

public static class FunctionalityApiExtensions
{
    public static void InitFunctionality(this WebApplicationBuilder builder)
    {
#if (UseApi)
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(FunctionalityApiExtensions).Assembly);
#endif
#if (UseGrpc)
        builder.Services.AddGrpc();
#endif

        builder.Services.AddBusinessServices();
        builder.Services.AddDataAccessService(builder.Configuration);
    }

    public static void MapFunctionalityGrpcServices(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGrpcService<GreeterService>();
    }
}