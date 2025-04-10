using Microsoft.Extensions.Logging;

namespace Infraestructure.Events.Handlers;

public record Request
    : INotificator;

public class Handler(
    ILogger<Handler> logger
) : IEventConsumer<Request>
{
    public Task ConsumeAsync(Request request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Consuming request {Request}", request);

        return Task.CompletedTask;
    }
}