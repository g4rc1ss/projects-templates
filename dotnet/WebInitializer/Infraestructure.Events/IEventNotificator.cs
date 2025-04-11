namespace Infraestructure.Events;

public interface IEventNotificator
{
#if (UseMemoryEvents)
    Task PublishAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : INotificator;
#else
    Task PublishAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default);
#endif
}