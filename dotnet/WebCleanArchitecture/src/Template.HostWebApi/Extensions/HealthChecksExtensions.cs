#if (SqlDatabase)
using Infraestructure.Database;
#endif

namespace Template.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
#if (UseApi)
        IHealthChecksBuilder healthChecks = builder.Services.AddHealthChecks();
#if (UseRedis || UseGarnet)
        healthChecks.AddRedis(builder.Configuration.GetConnectionString("Cache") ?? string.Empty);
#endif
#if (SqlDatabase)
        healthChecks.AddDbContextCheck<DatabaseContext>();
#endif
#if (UseAzServiceBus)
        // string? azureConnection = builder.Configuration.GetConnectionString("AzureServiceBus");
        // healthChecks.AddAzureServiceBusQueue(azureConnection, "serviceBusQueue");
        // healthChecks.AddAzureServiceBusTopic(azureConnection, "serviceBusTopic");
#elif (UseRabbitMQ)
        healthChecks.AddRabbitMQ();
#endif
#endif

#if (UseGrpc)
    IHealthChecksBuilder grpcHealthChecks = builder.Services.AddGrpcHealthChecks();
#if (UseRedis || UseGarnet)
    grpcHealthChecks.AddRedis(builder.Configuration.GetConnectionString("Cache") ?? string.Empty);
#endif
#if (SqlDatabase)
    grpcHealthChecks.AddDbContextCheck<DatabaseContext>();
#endif
#if (UseAzServiceBus)
    // string? azureConnection = builder.Configuration.GetConnectionString("AzureServiceBus");
    // grpcHealthChecks.AddAzureServiceBusQueue(azureConnection, "serviceBusQueue");
    // grpcHealthChecks.AddAzureServiceBusTopic(azureConnection, "serviceBusTopic");
#elif (UseRabbitMQ)
    grpcHealthChecks.AddRabbitMQ();
#endif
#endif
    }
}