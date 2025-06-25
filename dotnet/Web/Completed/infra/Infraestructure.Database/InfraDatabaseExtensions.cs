using Microsoft.Extensions.Hosting;
#if (SqlDatabase || NoSqlDatabase)
using Infraestructure.Database.Repository;
using Microsoft.Extensions.DependencyInjection;
#endif
#if (UseLitedb)
using LiteDB;
#endif
#if (SqlDatabase)
using Infraestructure.Database.HostedServices;
#endif
#if (UseSqlite)
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
#endif

namespace Infraestructure.Database;

public static class InfraDatabaseExtensions
{
    public static void AddDatabaseConfig(
        this IHostApplicationBuilder builder,
        Action<DatabaseSettings>? configureSettings = null
    )
    {
        DatabaseSettings settings = new();
        configureSettings?.Invoke(settings);

#if (SqlDatabase)
        builder.Services.AddSingleton<MigrationHostedService>();
        builder.Services.AddHostedService<MigrationHostedService>();
        
#endif
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
#if (UseAzureSql || UseSqlServer)
        builder.AddSqlServerDbContext<DatabaseContext>(
            "SqlServer",
            serverSettings =>
            {
                serverSettings.DisableTracing = settings.DisableTracing;
            }
        );

#endif
#if (UseSqlite)
        builder.AddSqliteDbContext<DatabaseContext>(settings);

#endif
#if (UseAzureCosmos)
        builder.Services.AddScoped<ICosmosdbPoc, CosmosdbPoc>();
        builder.AddAzureCosmosClient(
            "Cosmos",
            cosmosSettings =>
            {
                cosmosSettings.DisableTracing = settings.DisableTracing;
            }
        );

#endif
#if (UseLitedb)
        builder.Services.AddTransient<ILitedbPoc, LitedbPoc>();
        string? litedbConnection = builder.Configuration.GetConnectionString("Litedb");
        builder.Services.AddSingleton<ILiteDatabase>(new LiteDatabase(litedbConnection));

#endif
#if (UseMongodb)
        builder.Services.AddScoped<IMongoPoc, MongoPoc>();
        builder.AddMongoDBClient(
            "Mongo",
            dbSettings =>
            {
                dbSettings.DisableTracing = dbSettings.DisableTracing;
            }
        );
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
        this IHostApplicationBuilder builder,
        DatabaseSettings settings
    )
        where TContext : DbContext
    {
        string? connectionString = builder.Configuration.GetConnectionString("Sqlite");
        ArgumentNullException.ThrowIfNull(connectionString);

        if (!settings.DisableTracing)
        {
            builder.ConfigureOpenTelemetry();
        }

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
