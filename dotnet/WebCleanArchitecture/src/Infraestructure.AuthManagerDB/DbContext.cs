using Infraestructure.AuthManagerDB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.AuthManagerDB;

public class DbContext(DbContextOptions<DbContext> options)
    : IdentityDbContext<UserEntity, RoleEntity, int>(options)
{
    public DbSet<UserJwtTokensEntity> UserJwtTokens { get; set; }
    public override DbSet<UserEntity> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserEntity>().ToTable("Users");
        builder.Entity<RoleEntity>().ToTable("Roles");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

        builder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
    }
}