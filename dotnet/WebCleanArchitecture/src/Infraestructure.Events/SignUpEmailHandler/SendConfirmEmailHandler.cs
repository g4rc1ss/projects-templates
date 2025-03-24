using MediatR;

namespace Infraestructure.Events.SignUpEmailHandler;

public class SendConfirmEmailHandler(
) : INotificationHandler<ConfirmEmailRequest>
{
    public Task Handle(ConfirmEmailRequest notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}