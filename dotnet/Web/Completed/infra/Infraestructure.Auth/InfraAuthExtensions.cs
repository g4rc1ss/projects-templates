using Microsoft.Extensions.Hosting;
#if (UseJwt)
using Infraestructure.Auth.JwtManager;
#endif

#if (UseIdentity)
using Infraestructure.Auth.IdentityAuth;
#endif

namespace Infraestructure.Auth;

public static class InfraAuthExtensions
{
    public static void AddAuthenticationProtocol(this IHostApplicationBuilder builder)
    {
#if (UseJwt)
        builder.AddJwt();
#endif
#if (UseIdentity)
        builder.AddIdentityAuth();
#endif
    }
}
