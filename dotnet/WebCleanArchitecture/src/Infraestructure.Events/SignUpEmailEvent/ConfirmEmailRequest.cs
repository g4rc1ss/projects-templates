#if (UseMemoryEvents)
using MediatR;
#endif

namespace Infraestructure.Events.SignUpEmailEvent;

#if (UseMemoryEvents)
public record ConfirmEmailRequest(string Email, string UrlToConfirm) : INotification;
#else
public record ConfirmEmailRequest(string Email, string UrlToConfirm);
#endif