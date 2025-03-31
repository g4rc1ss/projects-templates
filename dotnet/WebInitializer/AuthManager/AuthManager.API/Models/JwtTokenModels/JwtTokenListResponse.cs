namespace AuthManager.API.Models.JwtTokenModels;

public record JwtTokenListResponse(IEnumerable<TokenInformation> Tokens);

public record TokenInformation(Guid Id, DateTime Expiration);