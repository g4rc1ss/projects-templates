using Infraestructure.Events.Messages.Models;
using Microsoft.Extensions.Logging;

namespace Infraestructure.Events.Consumer.Handlers;

public class Handler(ILogger<Handler> logger) : IEventConsumer<RequestMessage>
{
    public Task ConsumeAsync(RequestMessage request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Consuming request {RequestMessage}", request);

        return Task.CompletedTask;
    }
}

public class Handler2(ILogger<Handler2> logger) : IEventConsumer<RequestMessage>
{
    public Task ConsumeAsync(RequestMessage request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Consumer con Error");
        throw new NotImplementedException();
    }
}
