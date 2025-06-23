using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Auth.JwtManager.JwtTokenModels;

public record JwtRevokeTokenRequest
{
    [Required]
    public required string TokenId { get; init; }
}
