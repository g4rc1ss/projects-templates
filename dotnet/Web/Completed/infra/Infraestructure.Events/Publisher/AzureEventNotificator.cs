using System.Diagnostics;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Infraestructure.Events.Messages;

namespace Infraestructure.Events.Publisher;

public class AzureEventNotificator(ServiceBusClient serviceBusClient) : IEventNotificator
{
    public async Task PublishAsync<TRequest>(
        TRequest request,
        Dictionary<string, string> additionalProperties,
        CancellationToken cancellationToken = default
    )
        where TRequest : INotificatorRequest
    {
        string queuename = additionalProperties["AzQueueName"];

        ServiceBusSender? sender = serviceBusClient.CreateSender(queuename);

        MessageDiagnosticTraces traces = new()
        {
            TraceId = Activity.Current?.TraceId.ToString(),
            SpanId = Activity.Current?.SpanId.ToString(),
            ParentId = Activity.Current?.ParentId,
        };

        Message<TRequest> message = new(request, traces);

        ServiceBusMessage sbMessage = new(JsonSerializer.Serialize(message));

        await sender.SendMessageAsync(sbMessage, CancellationToken.None);
    }
}
