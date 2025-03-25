namespace Infraestructure.Events;

public interface IEventNotificator
{
    Task PublishAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default);
}