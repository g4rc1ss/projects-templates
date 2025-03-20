namespace AuthManager.Application.UsesCases.LoginCase;

public record LoginCredentials(string UserName, string Password, string? TotpCode, string? TotpRecoveryCode);