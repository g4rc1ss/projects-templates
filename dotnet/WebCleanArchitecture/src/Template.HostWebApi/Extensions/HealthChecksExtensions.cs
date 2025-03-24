#if (SqlDatabase)
using Infraestructure.Database;
#endif

namespace Template.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
#if (UseRedis || UseGarnet)
            .AddRedis(builder.Configuration.GetConnectionString("Cache") ?? string.Empty)
#endif
#if (SqlDatabase)
            .AddDbContextCheck<DatabaseContext>()
#endif
            ;
    }
}