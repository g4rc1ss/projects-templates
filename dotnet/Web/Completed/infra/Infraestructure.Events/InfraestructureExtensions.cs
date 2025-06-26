using Microsoft.Extensions.Hosting;
#if (UseMemoryEvents || UseAzServiceBus || UseRabbitMQ)
using Infraestructure.Events.Publisher;
using Microsoft.Extensions.DependencyInjection;
#endif

#if (UseMemoryEvents)
using Infraestructure.Events.Consumer;
using Infraestructure.Events.Messages;
using System.Threading.Channels;
using System.Reflection;
#endif

namespace Infraestructure.Events;

public static class InfraEventsExtensions
{
    public static void AddEventsServices(
        this IHostApplicationBuilder builder,
        Action<EventBusSettings>? configureSettings = null
    )
    {
        EventBusSettings settings = new();
        configureSettings?.Invoke(settings);

#if (UseMemoryEvents)
        builder.Services.AddScoped<IEventNotificator, MemoryEventNotificator>();

        IEnumerable<Assembly> assemblies = [typeof(InfraEventsExtensions).Assembly];
        assemblies = assemblies.Concat(settings.Assemblies ?? []);
        builder.AddConsumerServices(assemblies);

        if (!settings.DisableTracing)
        {
            builder.ConfigureOpenTelemetry();
        }

#endif
#if (UseAzServiceBus)
        builder.Services.AddScoped<IEventNotificator, AzureEventNotificator>();
        builder.AddAzureServiceBusClient(
            "AzureServiceBus",
            busSettings =>
            {
                busSettings.DisableTracing = settings.DisableTracing;
            }
        );

#endif
#if (UseRabbitMQ)
        builder.Services.AddScoped<IEventNotificator, RabbitEventNotificator>();
        builder.AddRabbitMQClient(
            "RabbitMQ",
            clientSettings =>
            {
                clientSettings.DisableTracing = settings.DisableTracing;
            }
        );
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
            // Obtenemos todas las clases que implementen
            IEnumerable<Type> notificators = assembly
                .GetTypes()
                .Where(x => x.IsClass && x.GetInterface(nameof(INotificatorRequest)) is not null);

            foreach (Type notificator in notificators)
            {
                // Obtenemos los handler del tipo que implementa la interfaz de notificacion para verificar si necesitamos crear workers
                Type handlerConsumer = typeof(IEventConsumer<>).MakeGenericType(notificator);
                List<Type> handlers =
                [
                    .. assembly
                        .GetTypes()
                        .Where(x => x.IsClass && x.GetInterface(handlerConsumer.Name) is not null)
                        .Where(x =>
                            x.GetInterfaces()
                                .Any(i => i.IsGenericType && i.FullName == handlerConsumer.FullName)
                        ),
                ];

                if (handlers.Count == 0)
                {
                    continue;
                }

                foreach (Type handler in handlers)
                {
                    typeof(InfraEventsExtensions)
                        .GetMethod(nameof(AddHandlers))
                        ?.MakeGenericMethod(notificator, handler)
                        .Invoke(null, [builder.Services]);
                }

                Type channelMessage = typeof(Message<>).MakeGenericType(notificator);
                typeof(InfraEventsExtensions)
                    .GetMethod(nameof(AddChannel))
                    ?.MakeGenericMethod(channelMessage)
                    .Invoke(null, [builder.Services]);

                Type consumerNotificator = typeof(ConsumerService<>).MakeGenericType(notificator);
                typeof(InfraEventsExtensions)
                    .GetMethod(nameof(AddWorkersService))
                    ?.MakeGenericMethod(consumerNotificator)
                    .Invoke(null, [builder.Services]);
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
        where TRequest : INotificatorRequest
        where TImplementation : class, IEventConsumer<TRequest>
    {
        services.AddSingleton<IEventConsumer<TRequest>, TImplementation>();
    }

    private static void ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddOpenTelemetry()
            .WithTracing(providerBuilder =>
                providerBuilder
                    .AddSource(EventsConst.CONSUMER_NAME)
                    .AddSource(EventsConst.PUBLISHER_NAME)
            );
    }
#endif
}