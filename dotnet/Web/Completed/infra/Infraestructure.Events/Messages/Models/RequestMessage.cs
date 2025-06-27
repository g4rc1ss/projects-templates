namespace Infraestructure.Events.Messages.Models;

public record RequestMessage(IDictionary<string, string>? AdditionalProperties = null)
    : INotificatorRequest;
