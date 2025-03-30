namespace AuthManager.Application.Contracts.InfraestructureContracts;

public interface IPasswordManager
{
    Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword);
    Task<bool> CheckPasswordAsync(string userName, string password);
    Task<string> GetResetTokenAsync(string userName);
    Task<bool> ConfirmPasswordTokenAsync(string userName, string token, string password);
}