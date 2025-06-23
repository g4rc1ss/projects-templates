#if (SqlDatabase && !UseSqlite)
using SimpleWeb.HostWebApi.Database;
#endif

namespace SimpleWeb.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
        IHealthChecksBuilder apiHealthChecks = builder.Services.AddHealthChecks();
#if (UseRedis || UseGarnet)
        apiHealthChecks.AddRedis(
            builder.Configuration.GetConnectionString("Cache") ?? string.Empty
        );
#endif
#if (SqlDatabase && !UseSqlite)
        apiHealthChecks.AddDbContextCheck<DatabaseContext>();
#endif

#if (UseGrpc)
        IHealthChecksBuilder grpcHealthChecks = builder.Services.AddGrpcHealthChecks();
#if (UseRedis || UseGarnet)
        grpcHealthChecks.AddRedis(
            builder.Configuration.GetConnectionString("Cache") ?? string.Empty
        );
#endif
#if (SqlDatabase && !UseSqlite)
        grpcHealthChecks.AddDbContextCheck<DatabaseContext>();
#endif
#endif
    }
}
