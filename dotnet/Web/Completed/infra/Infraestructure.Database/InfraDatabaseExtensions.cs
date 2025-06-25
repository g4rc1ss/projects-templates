using Microsoft.Extensions.Hosting;
#if (SqlDatabase || UseIdentity || NoSqlDatabase)
using Infraestructure.Database.Repository;
using Microsoft.Extensions.DependencyInjection;
#endif

#if (UseLitedb)
using LiteDB;
#endif
#if (SqlDatabase || UseIdentity)
using OpenTelemetry.Trace;
using Infraestructure.Database.HostedServices;
#endif
#if (UseSqlite)
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
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

#if (SqlDatabase || UseIdentity)
        builder.Services.AddSingleton<MigrationHostedService>();
        builder.Services.AddHostedService<MigrationHostedService>();

#if (UsePostgres)
#if (UseIdentity)
        builder.AddNpgsqlDbContext<IdentityDatabaseContext>(
#else
        builder.AddNpgsqlDbContext<DatabaseContext>(
#endif
            "Postgres",
            sqlSettings =>
            {
                sqlSettings.DisableTracing = settings.DisableTracing;
                sqlSettings.DisableMetrics = settings.DisableTracing;
            }
        );
#endif

#if (UseAzureSql || UseSqlServer)
#if (UseIdentity)
        builder.AddSqlServerDbContext<IdentityDatabaseContext>(
#else
        builder.AddSqlServerDbContext<DatabaseContext>(
#endif
            "SqlServer",
            serverSettings =>
            {
                serverSettings.DisableTracing = settings.DisableTracing;
            }
        );
#endif

#if (UseSqlite)
#if (UseIdentity)
        builder.AddSqliteDbContext<IdentityDatabaseContext>();
#else
        builder.AddSqliteDbContext<DatabaseContext>();
#endif
#endif

#if (UseIdentity)
        builder.Services.AddScoped<IIdentityUserRepository, IdentityUserPoc>();
#else
        builder.Services.AddScoped<IUserRepository, SqlUserPoc>();
#endif
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
