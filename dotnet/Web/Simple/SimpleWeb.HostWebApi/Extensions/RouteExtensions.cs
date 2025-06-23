#if (UseGrpc)
using SimpleWeb.HostWebApi.GrpcServices.Services;
#endif

namespace SimpleWeb.HostWebApi.Extensions;

public static class RouteExtensions
{
    internal static void MapRouteServices(this IEndpointRouteBuilder endpoints)
    {
#if (UseGrpc)
        endpoints.MapGrpcService<GreeterService>();
#endif
    }
}
