#if (SqlDatabase)
using Infraestructure.Database;
#endif

namespace CompletedWeb.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
#if (UseApi)
        IHealthChecksBuilder healthChecks = builder.Services.AddHealthChecks();
#elif (UseGrpc)
        IHealthChecksBuilder healthChecks = builder.Services.AddGrpcHealthChecks();
#endif
#if (UseRedis || UseGarnet)
        healthChecks.AddRedis(builder.Configuration.GetConnectionString("Cache") ?? string.Empty);
#endif
#if (SqlDatabase)
#if (UseIdentity)
        healthChecks.AddDbContextCheck<IdentityDatabaseContext>();
#else
        healthChecks.AddDbContextCheck<DatabaseContext>();
#endif
#endif
#if (UseAzServiceBus)
        // string? azureConnection = builder.Configuration.GetConnectionString("AzureServiceBus");
        // healthChecks.AddAzureServiceBusQueue(azureConnection, "serviceBusQueue");
        // healthChecks.AddAzureServiceBusTopic(azureConnection, "serviceBusTopic");
#elif (UseRabbitMQ)
        healthChecks.AddRabbitMQ();
#endif
    }
}
