using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Database.Contexts;

public class SqlServerContext(DbContextOptions<SqlServerContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(SqlServerContext).Assembly);
    }
}
