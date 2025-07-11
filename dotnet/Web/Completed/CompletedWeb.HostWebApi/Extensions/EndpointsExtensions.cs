#if (UseApi && UseLayerArchitecture)
using CompletedWeb.API;
#endif
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
        endpoints.MapGroup("api").AddEndpointFilter<EndpointValidatorFilter>().MapWebApi();
#endif
    }
}
