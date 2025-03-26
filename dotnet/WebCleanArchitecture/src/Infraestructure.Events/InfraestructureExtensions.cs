#if (UseMemoryEvents)
using MediatR;
#endif
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infraestructure.Events;

public static class InfraestructureEventsExtensions
{
    public static void AddEventsServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventNotificator, EventNotificator>();
#if (UseMemoryEvents)
        builder.Services.AddMediatR(typeof(InfraestructureEventsExtensions).Assembly);
#endif
#if (UseAzServiceBus)
        builder.AddAzureServiceBusClient("AzureServiceBus");
#endif
#if (UseRabbitMQ)
        builder.AddRabbitMQClient("RabbitMQ");
#endif
    }
}