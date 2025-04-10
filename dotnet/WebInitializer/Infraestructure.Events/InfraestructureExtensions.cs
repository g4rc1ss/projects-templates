#if (UseMemoryEvents)
using Infraestructure.Events.Handlers;
using System.Threading.Channels;
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
        builder.Services.AddSingleton(Channel.CreateUnbounded<Request>());
        builder.Services.AddHostedService<Handler>();
#endif
#if (UseAzServiceBus)
        builder.AddAzureServiceBusClient("AzureServiceBus");
#endif
#if (UseRabbitMQ)
        builder.AddRabbitMQClient("RabbitMQ");
#endif
    }
}