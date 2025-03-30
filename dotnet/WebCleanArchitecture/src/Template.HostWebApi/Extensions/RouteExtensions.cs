#if (UseIdentity)
using Infraestructure.Database.Entities;
#endif
#if ((UseCustomIdentity) && UseGrpc)
using AuthManager.GRPC;
#endif

namespace Template.HostWebApi.Extensions;

public static class RouteExtensions
{
    internal static void MapRouteServices(this IEndpointRouteBuilder endpoints)
    {
#if (UseIdentity)
        endpoints.MapGroup("auth")
            .MapIdentityApi<UserEntity>();
#endif
#if (UseCustomIdentity)
#if (UseGrpc)
        endpoints.MapFunctionalityGrpcServices();
#endif
#endif
    }
}