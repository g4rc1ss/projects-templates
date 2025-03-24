#if (UseJwt || UseIdentity)
using AuthManager.API;
#endif
#if (!DatabaseNone)
using Infraestructure.Database;
#endif
#if (!EventBusNone)
using Infraestructure.Events;
#endif

namespace Template.HostWebApi.Extensions;

internal static class ServiceExtensions
{
    internal static void InitTemplateHostConfig(this WebApplicationBuilder builder)
    {
        builder.AddHealthChecks();
        builder.Services.AddProblemDetails();
        builder.Services.AddOptions();

        builder.ConfigureOpenTelemetry();
        builder.Services.AddHttpClient();

        builder.Services.AddAuthenticationProtocol(builder.Configuration);
        builder.Services.ConfigureDataProtectionProvider(builder.Configuration);

#if (!DatabaseNone)
        builder.AddDatabaseConfig();
#endif
#if (!EventBusNone)
        builder.Services.AddEventsServices(builder.Configuration);
#endif
        InitializeFunctionalities(builder);
    }

    private static void InitializeFunctionalities(this WebApplicationBuilder builder)
    {
#if (UseJwt || UseIdentity)
        builder.InitAuthManager();
#endif
    }
}