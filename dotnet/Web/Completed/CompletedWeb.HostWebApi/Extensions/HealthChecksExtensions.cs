#if (UseSqlite)
using Infraestructure.Database;
#endif

namespace CompletedWeb.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
#if (UseApi)
        builder.Services.AddHealthChecks()
#if (UseSqlite)
        .AddDbContextCheck<DatabaseContext>()
#endif
        ;
#endif

#if (UseGrpc)
        builder.Services.AddGrpcHealthChecks()
#if (UseSqlite)
        .AddDbContextCheck<DatabaseContext>()
#endif
        ;
#endif
    }
}
