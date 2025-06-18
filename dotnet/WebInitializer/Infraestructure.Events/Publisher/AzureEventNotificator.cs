using System.Diagnostics;
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