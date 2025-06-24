using Microsoft.Extensions.Hosting;
#if (UseMemoryCache || UseDistributedCache)
using Infraestructure.Caching.CacheImplementation;
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Infraestructure.Caching;

public static class InfraCachingExtensions
{
    public const string CACHING_TRACE = "Caching.Trace";

    public static void AddCaching(
        this IHostApplicationBuilder builder,
        Action<CachingSettings>? configureSettings = null
    )
    {
        CachingSettings cachingSettings = new();
        configureSettings?.Invoke(cachingSettings);

#if (UseMemoryCache)
        if (!cachingSettings.DisableTracing)
        {
            builder.ConfigureOpenTelemetry();
        }

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<ICaching, MemoryCaching>();
#endif
#if (UseRedis || UseGarnet)
        builder.AddRedisDistributedCache(
            "Cache",
            settings =>
            {
                settings.DisableTracing = cachingSettings.DisableTracing;
            }
        );
        builder.Services.AddScoped<ICaching, DistributedCaching>();
#endif
    }

#if (UseMemoryCache)
    private static void ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddOpenTelemetry()
            .WithTracing(providerBuilder => providerBuilder.AddSource(CACHING_TRACE));
    }
#endif
}
