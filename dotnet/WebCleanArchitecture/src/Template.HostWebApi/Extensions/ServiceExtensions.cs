#if (UseCustomIdentity)
#if (UseApi)
using AuthManager.API;
#endif
#if (UseGrpc)
using AuthManager.GRPC;
#endif
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
        builder.AddEventsServices();
#endif
#if (UseMemoryCache)
        builder.Services.AddDistributedMemoryCache();
#elif (UseRedis || UseGarnet)
        builder.AddRedisDistributedCache("Cache");
#endif

        InitializeFunctionalities(builder);
    }

    private static void InitializeFunctionalities(this WebApplicationBuilder builder)
    {
#if (UseCustomIdentity)
#if (UseApi)
        builder.InitAuthManager();
#endif
#if (UseGrpc)
        builder.InitFunctionalityGrpc();
#endif
#endif
    }
}