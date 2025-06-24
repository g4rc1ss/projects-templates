using Microsoft.Extensions.Hosting;
#if (UseMemoryCache)
using OpenTelemetry;
#endif

#if (UseMemoryCache || UseGarnet || UseRedis)
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Infraestructure.Caching;

public static class InfraCachingExtensions
{
    public const string CACHING_TRACE = "Caching.Trace";

    public static void AddCaching(this IHostApplicationBuilder builder)
    {
#if (UseMemoryCache)
        builder.ConfigureOpenTelemetry();
        builder.Services.AddMemoryCache();
#elif (UseRedis || UseGarnet)
        builder.AddRedisDistributedCache("Cache");
#endif
#if (UseMemoryCache || UseGarnet || UseRedis)
        builder.Services.AddScoped<ICaching, DistributedCache>();
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
