#if (UseCustomIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace Infraestructure.Database.Entities;

public class RoleEntity
#if (UseCustomIdentity)
    : IdentityRole<int>
#endif
{
}