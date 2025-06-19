using Microsoft.Extensions.Hosting;
#if (UseLitedb)
using Microsoft.Extensions.Configuration;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
#endif
#if (SqlDatabase)
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
#endif

namespace Infraestructure.Database;

public static class InfraestructureDatabaseExtensions
{
    public static void AddDatabaseConfig(this IHostApplicationBuilder builder)
    {
#if (SqlDatabase)
        string? connectionString = builder.Configuration.GetConnectionString(
            nameof(DatabaseContext)
        );
        ArgumentNullException.ThrowIfNull(connectionString);

        builder.Services.AddDbContext<DatabaseContext>(
            dbContextBuilder =>
            {
#if (UseSqlite)
                dbContextBuilder.UseSqlite(connectionString);
#endif
            },
            ServiceLifetime.Transient
        );
#endif

#if (UseLitedb)
        string? litedbConnection = builder.Configuration.GetConnectionString(
            "litedb"
        );
        builder.Services.AddSingleton<ILiteDatabase>(new LiteDatabase(litedbConnection));
#endif
    }
}
