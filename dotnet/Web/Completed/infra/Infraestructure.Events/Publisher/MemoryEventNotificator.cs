using System.Diagnostics;
using System.Threading.Channels;
using Infraestructure.Events.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.Events.Publisher;

public class MemoryEventNotificator(IServiceProvider serviceProvider) : IEventNotificator
{
    public async Task PublishAsync<TRequest>(
        TRequest request,
        Dictionary<string, string> additionalProperties,
        CancellationToken cancellationToken = default
    )
        where TRequest : INotificatorRequest
    {
        using ActivitySource? tracingConsumer = new(EventsConst.PUBLISHER_NAME);
        using Activity? activity = tracingConsumer.CreateActivity(
            $"Publishing event",
            ActivityKind.Producer
        );
        activity?.Start();

        Channel<Message<TRequest>> channel = serviceProvider.GetRequiredService<
            Channel<Message<TRequest>>
        >();

        MessageDiagnosticTraces traces = new()
        {
            TraceId = Activity.Current?.TraceId.ToString(),
            SpanId = Activity.Current?.SpanId.ToString(),
            ParentId = Activity.Current?.ParentId,
        };
        Message<TRequest> message = new(request, traces);
        await channel.Writer.WriteAsync(message, cancellationToken);
    }
}
