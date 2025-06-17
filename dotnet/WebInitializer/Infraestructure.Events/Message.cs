namespace Infraestructure.Events;

public record Message<TRequest>(TRequest Request, MessageDiagnosticTraces Traces)
    where TRequest : INotificator;

public interface INotificator;

public record MessageDiagnosticTraces
{
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public string? ParentId { get; init; }
}