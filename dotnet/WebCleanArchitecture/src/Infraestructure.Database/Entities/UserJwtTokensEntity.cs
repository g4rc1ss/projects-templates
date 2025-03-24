namespace Infraestructure.Database.Entities;

public class UserJwtTokensEntity
{
    public string Id { get; set; }
    public int UserId { get; set; }
    public DateTime ExpirationUtc { get; set; }

    // Navigation
    public UserEntity User { get; set; }
}