using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
#if (UseMemoryCache || UseGarnet || UseRedis)
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Infraestructure.Caching;

public static class InfraestructureCachingExtensions
{
    public const string CACHING_TRACE = "CachingTrace";

    public static void AddCaching(this IHostApplicationBuilder builder)
    {
        OpenTelemetryBuilder telemetryBuilder = builder.Services.AddOpenTelemetry();
        telemetryBuilder.WithTracing(providerBuilder => providerBuilder.AddSource(CACHING_TRACE));
#if (UseMemoryCache)
        builder.Services.AddMemoryCache();
#elif (UseRedis || UseGarnet)
        builder.AddRedisDistributedCache("Cache");
#endif
#if (UseMemoryCache || UseGarnet || UseRedis)
        builder.Services.AddScoped<ICaching, DistributedCache>();
#endif
    }
}
