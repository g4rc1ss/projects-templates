using System.ComponentModel.DataAnnotations;

namespace AuthManager.API.Models.PasswordModels;

public class ConfirmPassword
{
    [Required] public string Password { get; set; }
    [Required] public string Token { get; set; }
}