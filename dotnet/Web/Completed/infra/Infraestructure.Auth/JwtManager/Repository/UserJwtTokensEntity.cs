namespace Infraestructure.Auth.JwtManager.Repository;

public class UserJwtTokensEntity
{
    public required string Id { get; set; }
    public int UserId { get; set; }
    public DateTime ExpirationUtc { get; set; }
}
