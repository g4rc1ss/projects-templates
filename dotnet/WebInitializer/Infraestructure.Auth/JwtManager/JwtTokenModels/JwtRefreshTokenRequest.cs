using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Auth.JwtManager.JwtTokenModels;

public record JwtRefreshTokenRequest
{
    [Required]
    public required string AccessToken { get; init; }
}
