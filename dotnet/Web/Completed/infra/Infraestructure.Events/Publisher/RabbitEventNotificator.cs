using System.Diagnostics;
using System.Text.Json;
using Infraestructure.Events.Messages;
using RabbitMQ.Client;

namespace Infraestructure.Events.Publisher;

public class RabbitEventNotificator(IConnection connection) : IEventNotificator
{
    public async Task PublishAsync<TRequest>(
        TRequest request,
        Dictionary<string, string> additionalProperties,
        CancellationToken cancellationToken = default
    )
        where TRequest : INotificatorRequest
    {
        await using IChannel? channel = await connection.CreateChannelAsync(
            cancellationToken: cancellationToken
        );
        BasicProperties? properties = new() { Persistent = true };

        MessageDiagnosticTraces traces = new()
        {
            TraceId = Activity.Current?.TraceId.ToString(),
            SpanId = Activity.Current?.SpanId.ToString(),
            ParentId = Activity.Current?.ParentId,
        };

        Message<TRequest> message = new(request, traces);

        string? exchange = additionalProperties["exchange"];
        string? routingKey = additionalProperties["routingKey"];

        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body: JsonSerializer.SerializeToUtf8Bytes(message),
            cancellationToken
        );
    }
}
