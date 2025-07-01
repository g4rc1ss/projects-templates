using Microsoft.EntityFrameworkCore;
using SimpleWeb.HostWebApi.Database.Entities;

namespace SimpleWeb.HostWebApi.Database;

public class SqliteContext(DbContextOptions<SqliteContext> options) : DbContext(options)
{
    public DbSet<WeatherForecastEntity> WeatherForecast { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqliteContext).Assembly);
    }
}
