using Microsoft.Extensions.Hosting;
#if (SqlDatabase || NoSqlDatabase)
using Infraestructure.Database.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
#endif
#if (UseLitedb)
using LiteDB;
#endif
#if (SqlDatabase)
using Infraestructure.Database.HostedServices;
using Microsoft.EntityFrameworkCore;
#endif

namespace Infraestructure.Database;

public static class InfraestructureDatabaseExtensions
{
    public static void AddDatabaseConfig(this IHostApplicationBuilder builder)
    {
#if (SqlDatabase)
        builder.Services.AddSingleton<MigrationHostedService>();
        builder.Services.AddHostedService<MigrationHostedService>();

        string? connectionString = builder.Configuration.GetConnectionString(
            nameof(DatabaseContext)
        );
        ArgumentNullException.ThrowIfNull(connectionString);

        builder.Services.AddDbContextPool<DatabaseContext>(dbContextBuilder =>
        {
#if (UsePostgres)
            dbContextBuilder.UseNpgsql(connectionString);
#elif (UseSqlServer)
            dbContextBuilder.UseSqlServer(connectionString);
#elif (UseAzureSql)
            dbContextBuilder.UseAzureSql(connectionString);
#elif (UseSqlite)
            dbContextBuilder.UseSqlite(connectionString);
#endif
        });

        builder.Services.AddDbContextFactory<DatabaseContext>(dbContextBuilder =>
        {
#if (UsePostgres)
            dbContextBuilder.UseNpgsql(connectionString);
#elif (UseSqlServer)
            dbContextBuilder.UseSqlServer(connectionString);
#elif (UseAzureSql)
            dbContextBuilder.UseAzureSql(connectionString);
#elif (UseSqlite)
            dbContextBuilder.UseSqlite(connectionString);
#endif
        });

        builder.Services.AddScoped<IUserRepository, SqlUserPoc>();
#endif

#if (UseAzureCosmos)
        builder.Services.AddScoped<ICosmosdbPoc, CosmosdbPoc>();
        builder.AddAzureCosmosClient("Templatedb");
#endif

#if (UseLitedb)
        builder.Services.AddTransient<ILitedbPoc, LitedbPoc>();
        string? litedbConnection = builder.Configuration.GetConnectionString("litedb");
        builder.Services.AddSingleton<ILiteDatabase>(new LiteDatabase(litedbConnection));
#endif

#if (UseMongodb)
        builder.Services.AddScoped<IMongoPoc, MongoPoc>();
        builder.AddMongoDBClient("mongo");
#endif
    }
}
