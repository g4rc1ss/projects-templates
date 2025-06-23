using SimpleWeb.HostWebApi.Database;

namespace SimpleWeb.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
        IHealthChecksBuilder healthChecks = builder.Services.AddHealthChecks();
        healthChecks.AddRedis(builder.Configuration.GetConnectionString("Cache") ?? string.Empty);
        healthChecks.AddDbContextCheck<DatabaseContext>();
    }
}
