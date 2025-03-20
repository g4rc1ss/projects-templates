using System.ComponentModel.DataAnnotations;

namespace AuthManager.API.Models.PasswordModels;

public record ChangePasswordRequest
{
    [Required] public string OldPassword { get; init; }
    [Required] public string NewPassword { get; init; }
}