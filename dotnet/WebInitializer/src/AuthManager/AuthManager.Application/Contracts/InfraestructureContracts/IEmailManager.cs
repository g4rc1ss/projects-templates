namespace AuthManager.Application.Contracts.InfraestructureContracts;

public interface IEmailManager
{
    Task<bool> CheckEmailConfirmAsync(string userName);
    Task<bool> ConfirmEmailAsync(string userName, string token);
    Task<string> GenerateEmailConfirmationTokenAsync(string userName);
}