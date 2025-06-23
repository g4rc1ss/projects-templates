#if (UseLayerArchitecture)
#if (UseApi)
using CompletedWeb.API;
#endif
#if (UseGrpc)
using CompletedWeb.Grpc;
#endif
#endif

#if (!AuthNone)
using Infraestructure.Auth;
#endif
#if (!StorageNone)
using Infraestructure.Storages;
#endif
#if (!DatabaseNone || UseIdentity)
using Infraestructure.Database;
#endif
#if (!EventBusNone)
using Infraestructure.Events;
#endif

namespace CompletedWeb.HostWebApi.Extensions;

internal static class ServiceExtensions
{
    internal static void InitTemplateHostConfig(this WebApplicationBuilder builder)
    {
        builder.AddHealthChecks();
        builder.Services.AddProblemDetails();
        builder.Services.AddOptions();

        builder.ConfigureOpenTelemetry();
        builder.Services.AddHttpClient();
        builder.Services.ConfigureDataProtectionProvider(builder.Configuration);

#if (!AuthNone)
        builder.AddAuthenticationProtocol();
#endif

#if (!DatabaseNone || UseIdentity)
        builder.AddDatabaseConfig();
#endif
#if (!EventBusNone)
        builder.AddEventsServices();
#endif
#if (!StorageNone)
        builder.AddStorages();
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
#if (UseLayerArchitecture)
#if (UseApi)
        builder.InitTemplateApi();
#endif
#if (UseGrpc)
        builder.InitTemplateGrpc();
#endif
#endif
    }
}
