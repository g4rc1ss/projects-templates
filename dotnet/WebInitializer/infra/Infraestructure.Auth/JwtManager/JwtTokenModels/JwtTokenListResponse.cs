namespace Infraestructure.Auth.JwtManager.JwtTokenModels;

public record JwtTokenListResponse(IEnumerable<TokenInformation> Tokens);

public record TokenInformation(Guid Id, DateTime Expiration);
