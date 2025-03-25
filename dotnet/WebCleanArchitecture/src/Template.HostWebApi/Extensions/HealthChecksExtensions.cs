#if (SqlDatabase)
using Infraestructure.Database;
#endif

namespace Template.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
        IHealthChecksBuilder healthChecks = builder.Services.AddHealthChecks();
#if (UseRedis || UseGarnet)
            healthChecks.AddRedis(builder.Configuration.GetConnectionString("Cache") ?? string.Empty);
#endif
#if (SqlDatabase)
            healthChecks.AddDbContextCheck<DatabaseContext>();
#endif
#if (UseAzServiceBus)
        string? azureConnection = builder.Configuration.GetConnectionString("AzureServiceBus");
        healthChecks.AddAzureServiceBusQueue(azureConnection, "serviceBusQueue");
        healthChecks.AddAzureServiceBusTopic(azureConnection, "serviceBusTopic");
#elif (UseRabbitMQ)
            healthChecks.AddRabbitMQ()
#endif
    }
}