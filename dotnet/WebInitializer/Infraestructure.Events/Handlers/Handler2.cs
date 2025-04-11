using Microsoft.Extensions.Logging;

namespace Infraestructure.Events.Handlers;

public class Handler2(
    ILogger<Handler2> logger
) : IEventConsumer<Request>
{
    public Task ConsumeAsync(Request request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Consuming request {Request}", request);

        return Task.CompletedTask;
    }
}