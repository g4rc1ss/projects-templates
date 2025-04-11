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
        builder.Services.AddChannel<Request>();
        builder.Services.AddSingleton<IEventConsumer<Request>, Handler>();
        builder.Services.AddSingleton<IEventConsumer<Request>, Handler2>();
        builder.Services.AddHostedService<ConsumerService<Request>>();
#endif
#if (UseAzServiceBus)
        builder.AddAzureServiceBusClient("AzureServiceBus");
#endif
#if (UseRabbitMQ)
        builder.AddRabbitMQClient("RabbitMQ");
#endif
    }

#if (UseMemoryEvents)
    private static void AddChannel<T>(this IServiceCollection services)
    {
        services.AddSingleton(Channel.CreateUnbounded<T>());
    }
#endif
}