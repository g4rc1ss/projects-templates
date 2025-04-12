namespace Infraestructure.Events;

public interface IEventConsumer<TRequest>
    where TRequest : INotificator
{
    public Task ConsumeAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface INotificator;

public record Message<TRequest>(TRequest Request, MessageDiagnosticTraces Traces)
    where TRequest : INotificator;

public record MessageDiagnosticTraces
{
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public string? ParentId { get; init; }
}
