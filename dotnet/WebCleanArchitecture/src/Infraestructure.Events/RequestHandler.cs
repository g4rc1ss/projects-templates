using MediatR;

namespace Infraestructure.Events;

public class RequestHandler : INotificationHandler<Request>
{
    public Task Handle(Request notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}