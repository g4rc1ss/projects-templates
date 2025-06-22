#if (UseIdentity)
using Infraestructure.Database.Entities;
#endif

namespace Template.HostWebApi.Extensions;

public static class RouteExtensions
{
    internal static void MapRouteServices(this IEndpointRouteBuilder endpoints)
    {
#if (UseIdentity)
        endpoints.MapGroup("auth").MapIdentityApi<IdentityUserEntity>();
#endif
    }
}
