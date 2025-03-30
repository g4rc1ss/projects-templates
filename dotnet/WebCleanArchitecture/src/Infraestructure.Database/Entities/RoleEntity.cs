#if (UseCustomIdentity || UseIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace Infraestructure.Database.Entities;

public class RoleEntity
#if (UseCustomIdentity || UseIdentity)
    : IdentityRole<int>
#endif
{
}