
namespace SimpleWeb.HostWebApi.Extensions;

internal static class ServiceExtensions
{
    internal static void InitSimpleWebHostConfig(this WebApplicationBuilder builder)
    {
        builder.AddHealthChecks();
        builder.Services.AddProblemDetails();
        builder.Services.AddOptions();

        builder.ConfigureOpenTelemetry();
        builder.Services.AddHttpClient();
        builder.Services.ConfigureDataProtectionProvider(builder.Configuration);
        
        builder.AddDatabaseConfig();
        builder.AddRedisDistributedCache("Cache");

        InitializeFunctionalities(builder);
    }

    private static void InitializeFunctionalities(this WebApplicationBuilder builder)
    {
    }
}
