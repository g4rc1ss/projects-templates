#if (UseMemoryEvents)
using MediatR;
#elif (UseAzServiceBus)
using Azure.Messaging.ServiceBus;
using System.Text.Json;
#elif (UseRabbitMQ)
using RabbitMQ.Client;
using System.Text.Json;
#endif
using Microsoft.Extensions.Configuration;

namespace Infraestructure.Events;

public class EventNotificatorPublisher(
#if (UseMemoryEvents)
    IMediator mediator,
#elif (UseAzServiceBus)
    ServiceBusClient serviceBusClient,
#elif (UseRabbitMQ)
    IConnection connection,
#endif
    IConfiguration configuration
) : IEventNotificator
{
    public async Task PublishAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
    {
#if (UseMemoryEvents)
        return await mediator.Publish(request, cancellationToken);
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