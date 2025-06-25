using Infraestructure.Events.Messages;

namespace Infraestructure.Events.Publisher;

public interface IEventNotificator
{
    Task PublishAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : INotificatorRequest;
}
