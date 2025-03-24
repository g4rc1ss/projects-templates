using System.ComponentModel.DataAnnotations;

namespace AuthManager.API.Models.SignUpModels;

public record SignUpRequest
{
    [Required] public string UserName { get; set; }
    [Required] public string Password { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
}