using Infraestructure.Events.Messages;

namespace Infraestructure.Events.Publisher;

public interface IEventNotificator
{
    Task PublishAsync<TRequest>(
        TRequest request,
        Dictionary<string, string> additionalProperties,
        CancellationToken cancellationToken = default
    )
        where TRequest : INotificatorRequest;
}

public static class EventNotificatorExtensions
{
    public static Task PublishAsync<TRequest>(
        this IEventNotificator notificator,
        TRequest request,
        CancellationToken cancellationToken = default
    )
        where TRequest : INotificatorRequest
    {
        return notificator.PublishAsync(request, [], cancellationToken);
    }
}
