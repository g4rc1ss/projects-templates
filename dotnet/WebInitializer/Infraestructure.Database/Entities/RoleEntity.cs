#if (UseIdentity)
using Microsoft.AspNetCore.Identity;
#endif

namespace Infraestructure.Database.Entities;

public class RoleEntity
#if (UseIdentity)
    : IdentityRole<int>
#endif
{ }
