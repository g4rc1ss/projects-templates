#if (UseCustomIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace Infraestructure.Database.Entities;

public class UserEntity
#if (UseCustomIdentity)
    : IdentityUser<int>
#endif
{
#if (!UseCustomIdentity)
    public int Id { get; set; }
#endif

#if (UseCustomIdentity || UseJwt)
    // Navigation
    public IEnumerable<UserJwtTokensEntity> JwtUserTokens { get; set; }
#endif
}