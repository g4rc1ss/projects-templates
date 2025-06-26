using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
#if (UseIdentity)
using Infraestructure.Auth.IdentityAuth;
#endif
#if (UseJwt)
using Infraestructure.Auth.JwtManager;
#endif

namespace Infraestructure.Auth;

public static class AuthEndpointsExtensions
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder authGroup = endpoints.MapGroup("api/auth");
#if (UseIdentity)
        authGroup.MapIdentityEndpoints();
#endif
#if (UseJwt)
        authGroup.MapJwt();
#endif
    }
}
