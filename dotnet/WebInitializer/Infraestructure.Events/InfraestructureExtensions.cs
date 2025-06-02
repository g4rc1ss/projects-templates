using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if (UseMemoryEvents)
using System.Threading.Channels;
using System.Reflection;
#endif

namespace Infraestructure.Events;

public static class InfraestructureEventsExtensions
{
    public static void AddEventsServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventNotificator, EventNotificator>();
#if (UseMemoryEvents)
        builder.AddConsumerServices([typeof(InfraestructureEventsExtensions).Assembly]);
#endif
#if (UseAzServiceBus)
        builder.AddAzureServiceBusClient("AzureServiceBus");
#endif
#if (UseRabbitMQ)
        builder.AddRabbitMQClient("RabbitMQ");
#endif
    }

#if (UseMemoryEvents)
    private static void AddConsumerServices(
        this IHostApplicationBuilder builder,
        IEnumerable<Assembly> assemblies
    )
    {
        foreach (Assembly assembly in assemblies)
        {
            IEnumerable<Type> notificators = assembly
                .GetTypes()
                .Where(x => x.IsClass && x.GetInterface(nameof(INotificator)) is not null);

            foreach (Type notificator in notificators)
            {
                Type channelMessage = typeof(Message<>).MakeGenericType(notificator);
                typeof(InfraestructureEventsExtensions)
                    .GetMethod(nameof(AddChannel))
                    ?.MakeGenericMethod(channelMessage)
                    .Invoke(null, [builder.Services]);

                Type consumerNotificator = typeof(ConsumerService<>).MakeGenericType(notificator);
                typeof(InfraestructureEventsExtensions)
                    .GetMethod(nameof(AddWorkersService))
                    ?.MakeGenericMethod(consumerNotificator)
                    .Invoke(null, [builder.Services]);

                Type handlerConsumer = typeof(IEventConsumer<>).MakeGenericType(notificator);
                IEnumerable<Type> handlers = assembly
                    .GetTypes()
                    .Where(x => x.IsClass && x.GetInterface(handlerConsumer.Name) is not null)
                    .Where(x =>
                        x.GetInterfaces()
                            .Any(i => i.IsGenericType && i.FullName == handlerConsumer.FullName)
                    );

                foreach (Type handler in handlers)
                {
                    typeof(InfraestructureEventsExtensions)
                        .GetMethod(nameof(AddHandlers))
                        ?.MakeGenericMethod(notificator, handler)
                        .Invoke(null, [builder.Services]);
                }
            }
        }
    }

    public static void AddChannel<T>(this IServiceCollection services)
    {
        services.AddSingleton(Channel.CreateUnbounded<T>());
    }

    public static void AddWorkersService<T>(this IServiceCollection services)
        where T : class, IHostedService
    {
        services.AddHostedService<T>();
    }

    public static void AddHandlers<TRequest, TImplementation>(this IServiceCollection services)
        where TRequest : INotificator
        where TImplementation : class, IEventConsumer<TRequest>
    {
        services.AddSingleton<IEventConsumer<TRequest>, TImplementation>();
    }
#endif
}
