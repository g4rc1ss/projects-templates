using System.Diagnostics;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infraestructure.Events;

public class ConsumerService<TRequest>(
    ILogger<ConsumerService<TRequest>> logger,
    Channel<Message<TRequest>> channel,
    IEnumerable<IEventConsumer<TRequest>> consumers
) : BackgroundService
    where TRequest : INotificator
{
    private readonly string _consumerName = typeof(TRequest).Name;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            while (channel.Reader.TryRead(out Message<TRequest>? request))
            {
                ActivityTraceId traceId = default;
                if (!string.IsNullOrEmpty(request?.Traces?.TraceId))
                {
                    traceId = ActivityTraceId.CreateFromString(request?.Traces?.TraceId);
                }

                ActivitySpanId spanId = default;
                if (!string.IsNullOrEmpty(request?.Traces?.SpanId))
                {
                    spanId = ActivitySpanId.CreateFromString(request?.Traces?.SpanId);
                }

                using ActivitySource? tracingConsumer = new(EventsConst.CONSUMER_NAME);
                using Activity? activity = tracingConsumer.CreateActivity(
                    $"{_consumerName} Consumer",
                    ActivityKind.Consumer
                );
                activity?.SetParentId(traceId, spanId, ActivityTraceFlags.Recorded);
                activity?.Start();
                try
                {
                    List<Task> parallelConsumers = [];
                    parallelConsumers.AddRange(
                        consumers.Select(x => x.ConsumeAsync(request!.Request, stoppingToken))
                    );

                    await Task.WhenAll(parallelConsumers);
                }
                catch (Exception e)
                {
                    logger.LogError(
                        e,
                        "Exception occured on consumer {ConsumerName}",
                        typeof(TRequest).Name
                    );
                }
            }
        }
    }
}
