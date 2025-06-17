using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Infraestructure.Events.Publisher;

public class AzureEventNotificator(ServiceBusClient serviceBusClient, IConfiguration configuration)
    : IEventNotificator
{
    public async Task PublishAsync<TRequest>(
        TRequest request,
        CancellationToken cancellationToken = default
    )
        where TRequest : INotificator
    {
        string? queuename = configuration.GetSection("ServiceBusConfig")["QueueName"];
        ServiceBusSender? sender = serviceBusClient.CreateSender(queuename);

        ServiceBusMessage message = new(JsonSerializer.Serialize(request));

        await sender.SendMessageAsync(message, CancellationToken.None);
    }
}