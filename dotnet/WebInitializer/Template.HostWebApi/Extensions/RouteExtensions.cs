using Infraestructure.Auth;

namespace Template.HostWebApi.Extensions;

public static class RouteExtensions
{
    internal static void MapRouteServices(this IEndpointRouteBuilder endpoints)
    {
#if (!AuthNone)
        endpoints.MapAuthEndpoints();
#endif
    }
}
