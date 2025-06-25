#if (SqlDatabase || NoSqlDatabase)
using SimpleWeb.HostWebApi.Database.Repository;
#endif
#if (UseLitedb)
using LiteDB;
#endif
#if (SqlDatabase)
using Microsoft.EntityFrameworkCore;
using SimpleWeb.HostWebApi.Database;
using SimpleWeb.HostWebApi.Database.HostedServices;
#endif

namespace SimpleWeb.HostWebApi.Extensions;

public static class DatabaseExtensions
{
    public static void AddDatabaseConfig(this IHostApplicationBuilder builder)
    {
#if (SqlDatabase)
        builder.Services.AddSingleton<MigrationHostedService>();
        builder.Services.AddHostedService<MigrationHostedService>();

#if (UsePostgres)
        builder.AddNpgsqlDbContext<DatabaseContext>(
            "Postgres",
            sqlSettings =>
            {
                sqlSettings.DisableTracing = settings.DisableTracing;
                sqlSettings.DisableMetrics = settings.DisableTracing;
            }
        );
#endif

#if (UseSqlite)
        builder.AddSqliteDbContext<DatabaseContext>();
#endif

        builder.Services.AddScoped<IUserRepository, SqlUserPoc>();
#endif
#if (UseLitedb)
        builder.Services.AddTransient<ILitedbPoc, LitedbPoc>();
        string? litedbConnection = builder.Configuration.GetConnectionString("Litedb");
        builder.Services.AddSingleton<ILiteDatabase>(new LiteDatabase(litedbConnection));
#endif
#if (UseMongodb)
        builder.Services.AddScoped<IMongoPoc, MongoPoc>();
        builder.AddMongoDBClient("Mongo");
#endif
    }

#if (UseSqlite)
    private static void ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddOpenTelemetry()
            .WithTracing(providerBuilder =>
                providerBuilder.AddEntityFrameworkCoreInstrumentation(options =>
                {
                    // Guardamos las consultas generadas por EF
                    options.SetDbStatementForText = true;
                })
            );
    }

    private static void AddSqliteDbContext<TContext>(
        this IHostApplicationBuilder builder
    )
        where TContext : DbContext
    {
        string? connectionString = builder.Configuration.GetConnectionString("Sqlite");
        ArgumentNullException.ThrowIfNull(connectionString);

        builder.ConfigureOpenTelemetry();

        builder.Services.AddDbContext<TContext>(
            dbContextBuilder =>
            {
                dbContextBuilder.UseSqlite(connectionString);
            },
            ServiceLifetime.Transient
        );
    }
#endif
}
