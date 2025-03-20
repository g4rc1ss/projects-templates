using System.ComponentModel.DataAnnotations;

namespace AuthManager.API.Models.LoginModels;

public record LoginRequest
{
    [Required] public string Username { get; set; }

    [Required] public string Password { get; set; }

    public string? TotpCode { get; set; }

    public string? TotpRecoveryCode { get; set; }
}