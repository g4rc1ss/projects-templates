namespace Infraestructure.Events;

public interface IEventConsumer<TRequest>
    where TRequest : INotificatorRequest
{
    public Task ConsumeAsync(TRequest request, CancellationToken cancellationToken = default);
}
