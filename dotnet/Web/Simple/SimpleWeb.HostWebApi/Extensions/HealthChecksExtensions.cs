using SimpleWeb.HostWebApi.Database;

namespace SimpleWeb.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddHealthChecks()
            .AddRedis(builder.Configuration.GetConnectionString("Cache") ?? string.Empty)
            .AddDbContextCheck<DatabaseContext>();
        
#if (UseGrpc)
        builder
            .Services.AddGrpcHealthChecks()
            .AddRedis(builder.Configuration.GetConnectionString("Cache") ?? string.Empty)
            .AddDbContextCheck<DatabaseContext>();
#endif
    }
}
