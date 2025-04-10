namespace Infraestructure.Events.SignUpEmailEvent;

public record ConfirmEmailRequest(string Email, string UrlToConfirm);