#if (UseJwt || UseIdentity)
using AuthManager.API;
#endif
using Functionality.API;
using Infraestructure.Database;
using Infraestructure.Events;

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

        builder.AddDatabaseConfig();
        builder.Services.AddEventsServices(builder.Configuration);

        InitializeFunctionalities(builder);
    }

    private static void InitializeFunctionalities(this WebApplicationBuilder builder)
    {
        builder.InitFunctionality();
#if (UseJwt || UseIdentity)
        builder.InitAuthManager();
#endif
    }
}