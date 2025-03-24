using Microsoft.Extensions.Configuration;
#if (UsePostgres)
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
#endif
using Microsoft.Extensions.Hosting;

namespace Infraestructure.Database;

public static class InfraestructureDatabaseExtensions
{
    public static void AddDatabaseConfig(this IHostApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString(nameof(DatabaseContext));
        ArgumentNullException.ThrowIfNull(connectionString);

#if (UsePostgres)
        builder.Services.AddDbContextPool<DatabaseContext>(builder =>
            builder.UseNpgsql(connectionString)
        );

        builder.Services.AddDbContextFactory<DatabaseContext>(builder =>
            builder.UseNpgsql(connectionString)
        );
#endif
    }
}