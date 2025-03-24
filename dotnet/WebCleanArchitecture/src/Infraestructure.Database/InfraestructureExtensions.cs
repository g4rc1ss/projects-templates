using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infraestructure.Database;

public static class InfraestructureDatabaseExtensions
{
    public static void AddDatabaseConfig(this IHostApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString(nameof(DatabaseContext));
        ArgumentNullException.ThrowIfNull(connectionString);

        builder.Services.AddDbContextPool<DatabaseContext>(builder =>
            builder.UseNpgsql(connectionString)
        );

        builder.Services.AddDbContextFactory<DatabaseContext>(builder =>
            builder.UseNpgsql(connectionString)
        );
    }
}