using SimpleWeb.HostWebApi.UsesCases;

namespace SimpleWeb.HostWebApi.Extensions;

internal static class ServiceExtensions
{
    internal static void InitWebHostConfig(this WebApplicationBuilder builder)
    {
        builder.AddHealthChecks();
        builder.Services.AddProblemDetails();
        builder.Services.AddOptions();

        builder.ConfigureOpenTelemetry();
        
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
        
        builder.ConfigureDataProtectionProvider();

#if (!DatabaseNone)
        builder.AddDatabaseConfig();
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
        builder.Services.AddScoped<Example>();
    }
}
