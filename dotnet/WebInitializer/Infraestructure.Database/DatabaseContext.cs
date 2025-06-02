using Microsoft.EntityFrameworkCore;
#if (UseIdentity)
using Infraestructure.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
#endif


namespace Infraestructure.Database;

#if (UseIdentity)
public class DatabaseContext(DbContextOptions<DatabaseContext> options)
    : IdentityDbContext<UserEntity, RoleEntity, int>(options)
#else
public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
#endif
{
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
