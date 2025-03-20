using System.ComponentModel.DataAnnotations;

namespace AuthManager.API.Models.JwtTokenModels;

public record JwtRevokeTokenRequest
{
    [Required] public string TokenId { get; init; }
}