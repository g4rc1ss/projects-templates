#if (UseLayerArchitecture)
#if (UseApi)
using CompletedWeb.API;
#endif
#if (UseGrpc)
using CompletedWeb.Grpc;
#endif
#endif
#if (UseCache)
using Infraestructure.Caching;
#endif
#if (UseJwt)
using Infraestructure.Auth.JwtManager;
#endif
#if (UseApiKey)
using Infraestructure.Auth.ApiKey;
#endif
#if (UseAzureAD)
using Infraestructure.Auth.AzureAD;
#endif
#if (!StorageNone)
using Infraestructure.Storages;
#endif
#if (!DatabaseNone)
using Infraestructure.Database;
#endif
#if (!EventBusNone)
using Infraestructure.Events;
#endif

namespace CompletedWeb.HostWebApi.Extensions;

internal static class ServiceExtensions
{
    internal static void InitHostWebConfig(this WebApplicationBuilder builder)
    {
        builder.AddHealthChecks();
        builder.Services.AddProblemDetails();
        builder.Services.AddOptions();

        builder.ConfigureOpenTelemetry();
        builder.Services.AddHttpClient();
        builder.ConfigureDataProtectionProvider();

#if (UseIdentity)
        // builder.AddIdentityAuth<>();
#endif
#if (UseAzureAD)
        builder.AddAzureAd();
#endif
#if (UseApiKey)
        builder.AddApiKey();
#endif
#if (UseJwt)
        builder.AddJwt();

#endif
#if (!DatabaseNone)
        builder.AddDatabaseConfig();

#endif
#if (!EventBusNone)
        builder.AddEventsServices();

#endif
#if (!StorageNone)
        builder.AddStorages();

#endif
#if (UseCache)
        builder.AddCaching();

#endif
        InitializeFunctionalities(builder);
    }

    private static void InitializeFunctionalities(this WebApplicationBuilder builder)
    {
#if (UseLayerArchitecture)
#if (UseApi)
        builder.InitWebApi();
#endif
#if (UseGrpc)
        builder.InitWebGrpc();
#endif
#endif
    }
}
