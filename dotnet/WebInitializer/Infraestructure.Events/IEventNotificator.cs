namespace Infraestructure.Events;

public interface IEventNotificator
{
    Task PublishAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default);
}

public interface IEventConsumer<TRequest>
    where TRequest : INotificator
{
    public Task ConsumeAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface INotificator;