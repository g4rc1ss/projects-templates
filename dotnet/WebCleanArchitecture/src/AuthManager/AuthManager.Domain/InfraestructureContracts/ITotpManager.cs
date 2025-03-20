namespace AuthManager.Domain.InfraestructureContracts;

public interface ITotpManager
{
    Task<bool> HasTwoFactorAuth(string userName);
    Task<bool> VerifyTotpCodeAsync(string userName, string? totpCode);
    Task<bool> CheckTotpRecoveryCodes(string userName, string recoveryCode);
    Task<bool> VerifyTwoFactorTokenAsync(string userName, string totpCode);
    Task<IEnumerable<string>?> GenerateRecoveryCodesAsync(string userName, int numberOfCodes);
    Task<bool> SetTwoFactorAsync(string userName, bool enable);
    Task<string?> GetAuthenticatorKeyAsync(string userName);
}