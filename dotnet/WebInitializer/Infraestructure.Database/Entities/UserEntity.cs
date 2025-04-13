#if (UseIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace Infraestructure.Database.Entities;

public class UserEntity
#if (UseIdentity)
    : IdentityUser<int>
#endif
{
#if (!UseIdentity)
    public int Id { get; set; }
#endif
}