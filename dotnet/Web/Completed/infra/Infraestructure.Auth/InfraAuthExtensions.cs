using Microsoft.Extensions.Hosting;
#if (UseJwt)
using Infraestructure.Auth.JwtManager;
#endif
#if (UseIdentity)

#endif

#if (UseIdentity || UseJwt)
using Microsoft.Extensions.DependencyInjection;
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

#endif
    }
}
