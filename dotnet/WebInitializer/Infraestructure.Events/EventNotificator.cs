#if (UseMemoryEvents)
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;
#elif (UseAzServiceBus)
using Azure.Messaging.ServiceBus;
using System.Text.Json;
#elif (UseRabbitMQ)
using RabbitMQ.Client;
using System.Text.Json;
#endif
using Microsoft.Extensions.Configuration;

namespace Infraestructure.Events;

public class EventNotificator(
#if (UseMemoryEvents)
    IServiceProvider serviceProvider,
#elif (UseAzServiceBus)
    ServiceBusClient serviceBusClient,
#elif (UseRabbitMQ)
    IConnection connection,
#endif
    IConfiguration configuration
) : IEventNotificator
{
    public async Task PublishAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
#if (UseMemoryEvents)
        where TRequest : INotificator
#endif
    {
#if (UseMemoryEvents)
        Channel<TRequest> channel = serviceProvider.GetRequiredService<Channel<TRequest>>();
        await channel.Writer.WriteAsync(request, cancellationToken);
#elif (UseAzServiceBus)
        string? queuename = configuration.GetSection("ServiceBusConfig")["QueueName"];
        ServiceBusSender? sender = serviceBusClient.CreateSender(queuename);

        ServiceBusMessage message = new(JsonSerializer.Serialize(request));

        await sender.SendMessageAsync(message, CancellationToken.None);
#elif (UseRabbitMQ)
        using IModel? model = connection.CreateModel();
        IBasicProperties? properties = model.CreateBasicProperties();
        properties.Persistent = true;

        model.BasicPublish(exchange: "",
            routingKey: "",
            basicProperties: properties,
            body: JsonSerializer.SerializeToUtf8Bytes(request));
#else

        throw new NotImplementedException();
#endif
    }
}