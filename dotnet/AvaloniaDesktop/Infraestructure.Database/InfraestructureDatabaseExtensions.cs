using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
#if (UseMongodb)
using Infraestructure.Database.Repository;
using Infraestructure.Database.Entities;
#endif

#if (SqlDatabase)
using Infraestructure.Database.Repository;
using Microsoft.EntityFrameworkCore;
#endif

namespace Infraestructure.Database;

public static class InfraestructureDatabaseExtensions
{
    public static void AddDatabaseConfig(this IHostApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString(
            nameof(DatabaseContext)
        );
#if (SqlDatabase)
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
        builder.Services.AddSingleton<ILiteDatabase>(new LiteDatabase(connectionString));
#endif
    }
}
