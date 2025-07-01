using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Database.Contexts;

public class SqlServerContext(DbContextOptions<SqlServerContext> options) : DbContext(options)
{
    public DbSet<WeatherForecastEntity> WeatherForecast { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlServerContext).Assembly);
    }
}
