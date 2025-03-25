using MediatR;

namespace Infraestructure.Events.Handlers;

public record Request : INotification;

public class Handler : INotificationHandler<Request>
{
    public Task Handle(Request notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}