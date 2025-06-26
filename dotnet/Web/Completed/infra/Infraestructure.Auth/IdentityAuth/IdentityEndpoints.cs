using Infraestructure.Auth.IdentityAuth.IdentityEntities;
using Microsoft.AspNetCore.Routing;

namespace Infraestructure.Auth.IdentityAuth;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapIdentityApi<IdentityUserEntity>();
    }
}
