#if (UseJwt || UseIdentity)
using Infraestructure.Database.Entities;
#endif
#if (UseIdentity)
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
#endif
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Database;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> options
)
#if (UseIdentity)
    : IdentityDbContext<UserEntity, RoleEntity, int>(options)
#else
    : DbContext(options)
#endif
{
#if (UseJwt || UseIdentity)
    public DbSet<UserJwtTokensEntity> UserJwtTokens { get; set; }
#endif

#if (UseIdentity)
    public override DbSet<UserEntity> Users { get; set; }
#endif

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
#if (UseIdentity)
        builder.Entity<UserEntity>().ToTable("Users");
        builder.Entity<RoleEntity>().ToTable("Roles");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
#endif
        builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}