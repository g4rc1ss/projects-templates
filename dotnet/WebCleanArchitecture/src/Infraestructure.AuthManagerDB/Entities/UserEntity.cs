using Microsoft.AspNetCore.Identity;

namespace Infraestructure.AuthManagerDB.Entities;

public class UserEntity : IdentityUser<int>
{
        
    // Navigation
    public IEnumerable<UserJwtTokensEntity> JwtUserTokens { get; set; }
}