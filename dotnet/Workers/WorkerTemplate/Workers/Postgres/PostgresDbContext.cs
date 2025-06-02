using Microsoft.EntityFrameworkCore;

namespace WorkerTemplate.Workers.Postgres;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    public DbSet<Entity> Entity { get; set; }
}
