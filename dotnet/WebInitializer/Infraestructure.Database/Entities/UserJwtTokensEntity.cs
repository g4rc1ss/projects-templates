namespace Infraestructure.Database.Entities;

public class UserJwtTokensEntity
{
    public int UserId { get; set; }
    public required string Id { get; set; }
    public DateTime ExpirationUtc { get; set; }
}