using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Infraestructure.Events.Handlers;

public class ConsumerService<TRequest>(
    ILogger<ConsumerService<TRequest>> logger,
    Channel<TRequest> channel,
    IEnumerable<IEventConsumer<TRequest>> consumers
) : BackgroundService
    where TRequest : INotificator
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            try
            {
                while (channel.Reader.TryRead(out TRequest? request))
                {
                    List<Task> parallelConsumers = [];
                    parallelConsumers.AddRange(consumers.Select(x =>
                        x.ConsumeAsync(request, stoppingToken)));

                    await Task.WhenAll(parallelConsumers);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception occured on consumer {ConsumerName}", typeof(TRequest).Name);
            }
        }
    }
}