using MediatR;

namespace Infraestructure.Events.SignUpEmailHandler;

public record ConfirmEmailRequest(string Email, string UrlToConfirm) : INotification;