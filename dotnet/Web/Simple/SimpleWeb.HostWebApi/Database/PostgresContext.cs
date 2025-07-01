using Microsoft.EntityFrameworkCore;
using SimpleWeb.HostWebApi.Database.Entities;

namespace SimpleWeb.HostWebApi.Database;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    public DbSet<WeatherForecastEntity> WeatherForecast { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresContext).Assembly);
    }
}
