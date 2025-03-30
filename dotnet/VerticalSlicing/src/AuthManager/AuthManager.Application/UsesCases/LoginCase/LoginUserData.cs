namespace AuthManager.Application.UsesCases.LoginCase;

public record LoginUserData(int UserId, string UserName, string Email, IEnumerable<string>? Roles);