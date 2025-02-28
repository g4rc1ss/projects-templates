using MediatR;

namespace Infraestructure.Events;

public record Request() : INotification;
