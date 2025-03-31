#if (UseCustomIdentity || UseIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace Infraestructure.Database.Entities;

public class UserEntity
#if (UseCustomIdentity || UseIdentity)
    : IdentityUser<int>
#endif
{
#if (!UseCustomIdentity && !UseIdentity)
    public int Id { get; set; }
#endif

#if (UseCustomIdentity || UseIdentity || UseJwt)
    // Navigation
    public IEnumerable<UserJwtTokensEntity> JwtUserTokens { get; set; }
#endif
}