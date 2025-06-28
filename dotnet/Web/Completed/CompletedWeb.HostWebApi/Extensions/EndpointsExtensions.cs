#if (UseApi && UseLayerArchitecture)
#endif
using CompletedWeb.API;
#if (UseGrpc && UseLayerArchitecture)
using CompletedWeb.Grpc;
#endif

namespace CompletedWeb.HostWebApi.Extensions;

public static class EndpointsExtensions
{
    internal static void MapHostEndpointsServices(this IEndpointRouteBuilder endpoints)
    {
#if (UseGrpc && UseLayerArchitecture)
        endpoints.MapWebGrpc();
#endif

#if (UseApi && UseLayerArchitecture)
#endif
        endpoints
            .MapGroup("api")
            .AddEndpointFilter(
                async (context, next) =>
                {
                    object? value = await next(context);

                    return value;
                }
            )
            .MapWebApi();
    }
}
