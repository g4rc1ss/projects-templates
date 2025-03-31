#if (SqlDatabase)
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
#endif
using Microsoft.Extensions.Hosting;

namespace Infraestructure.Database;

public static class InfraestructureDatabaseExtensions
{
    public static void AddDatabaseConfig(this IHostApplicationBuilder builder)
    {
#if (SqlDatabase)
        string? connectionString = builder.Configuration.GetConnectionString(nameof(DatabaseContext));
        ArgumentNullException.ThrowIfNull(connectionString);
        builder.Services.AddDbContextPool<DatabaseContext>(builder =>
        {
#if (UsePostgres)
            builder.UseNpgsql(connectionString);
#elif (UseSqlServer)
            builder.UseSqlServer(connectionString);
#elif (UseAzureSql)
            builder.UseAzureSql(connectionString);
#endif
        });

        builder.Services.AddDbContextFactory<DatabaseContext>(builder =>
        {
#if (UsePostgres)
            builder.UseNpgsql(connectionString);
#elif (UseSqlServer)
            builder.UseSqlServer(connectionString);
#elif (UseAzureSql)
            builder.UseAzureSql(connectionString);
#endif
        });
#endif
    }
}