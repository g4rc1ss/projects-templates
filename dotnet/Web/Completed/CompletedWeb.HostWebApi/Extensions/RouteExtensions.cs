#if (!AuthNone)
using Infraestructure.Auth;
#endif

namespace CompletedWeb.HostWebApi.Extensions;

public static class RouteExtensions
{
    internal static void MapRouteServices(this IEndpointRouteBuilder endpoints)
    {
#if (!AuthNone)
        endpoints.MapAuthEndpoints();
#endif
    }
}
