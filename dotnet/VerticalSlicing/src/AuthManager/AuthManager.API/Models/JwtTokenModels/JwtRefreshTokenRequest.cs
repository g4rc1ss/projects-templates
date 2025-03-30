using System.ComponentModel.DataAnnotations;

namespace AuthManager.API.Models.JwtTokenModels;

public record JwtRefreshTokenRequest
{
    [Required] public string AccessToken { get; init; }
}