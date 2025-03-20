namespace AuthManager.Domain.ErrorsCode;

public class UserManagerErrors
{
    public const string InvalidCredentialsCode = "User.Credentials.Invalid";
    public const string InvalidCredentialsDesc = "User credentials are invalid";

    public const string TotpRequiredCode = "User.Totp.Required";
    public const string TotpRequiredDesc = "User has configure the Totp";

    public const string TotpVerifyTotpCode = "User.Totp.VerifyTotp";
    public const string TotpVerifyTotpDesc = "Totp validation error";

    public const string LockedCode = "User.Locked";
    public const string LockedDesc = "User is locked";


    public const string EmailNotConfirmedCode = "User.Email.NotConfirmed";
    public const string EmailNotConfirmedDesc = "Email is not confirm";

    public const string InvalidRecoveryCodesCode = "User.RecoveryCode.Invalid";
    public const string InvalidRecoveryCodesDesc = "The recovery codes are invalid";
}