using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Database.Contexts;

public class SqliteContext(DbContextOptions<SqliteContext> options) : DbContext(options)
{
    public DbSet<WeatherForecastEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteContext).Assembly);
    }
}
