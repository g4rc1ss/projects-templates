using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Database.EntityConfiguration;

public class UserJwtTokensConfiguration : IEntityTypeConfiguration<UserJwtTokensEntity>
{
    public void Configure(EntityTypeBuilder<UserJwtTokensEntity> builder)
    {
        builder.ToTable("UserJwtTokens");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        builder.Property(x => x.ExpirationUtc)
            .IsRequired();

#if (UseCustomIdentity || UseJwt)
        // Relation 1:N con Users
        builder.HasOne<UserEntity>(x => x.User)
            .WithMany(x => x.JwtUserTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
#endif
    }
}