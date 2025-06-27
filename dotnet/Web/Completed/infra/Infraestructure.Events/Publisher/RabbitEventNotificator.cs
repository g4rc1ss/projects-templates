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
        using IModel? model = connection.CreateModel();
        IBasicProperties? properties = model.CreateBasicProperties();
        properties.Persistent = true;

        MessageDiagnosticTraces traces = new()
        {
            TraceId = Activity.Current?.TraceId.ToString(),
            SpanId = Activity.Current?.SpanId.ToString(),
            ParentId = Activity.Current?.ParentId,
        };

        Message<TRequest> message = new(request, traces);

        string? exchange = additionalProperties["exchange"];
        string? routingKey = additionalProperties["routingKey"];

        model.BasicPublish(
            exchange: exchange,
            routingKey: routingKey,
            basicProperties: properties,
            body: JsonSerializer.SerializeToUtf8Bytes(message)
        );
    }
}
