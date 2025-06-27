namespace Infraestructure.Events.Messages;

public record Message<TRequest>(TRequest Request, MessageDiagnosticTraces Traces)
    where TRequest : INotificatorRequest;

public interface INotificatorRequest
{
    IDictionary<string, string>? AdditionalProperties { get; init; }
}

public record MessageDiagnosticTraces
{
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public string? ParentId { get; init; }
}
