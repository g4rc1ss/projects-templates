#if ((UseIdentity || UseJwt) && UseGrpc)
using AuthManager.GRPC;
#endif

namespace Template.HostWebApi.Extensions;

public static class RouteExtensions
{
    internal static void MapRouteServices(this IEndpointRouteBuilder endpoints)
    {
#if (UseJwt || UseIdentity)
#if (UseGrpc)
        endpoints.MapFunctionalityGrpcServices();
#endif
#endif
    }
}