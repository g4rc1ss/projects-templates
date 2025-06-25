using Microsoft.EntityFrameworkCore;
using SimpleWeb.HostWebApi.Database.Entities;

namespace SimpleWeb.HostWebApi.Database;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(PostgresContext).Assembly);
    }
}
