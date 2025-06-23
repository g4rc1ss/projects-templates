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
        string? connectionString = builder.Configuration.GetConnectionString(
            nameof(DatabaseContext)
        );
        ArgumentNullException.ThrowIfNull(connectionString);

        builder.Services.AddDbContextPool<DatabaseContext>(dbContextBuilder =>
        {
#if (UsePostgres)
            dbContextBuilder.UseNpgsql(connectionString);
#endif
#if (UseSqlite)
            dbContextBuilder.UseSqlite(connectionString);
#endif
        });

        builder.Services.AddDbContextFactory<DatabaseContext>(dbContextBuilder =>
        {
#if (UsePostgres)
            dbContextBuilder.UseNpgsql(connectionString);
#endif
#if (UseSqlite)
            dbContextBuilder.UseSqlite(connectionString);
#endif
        });

        builder.Services.AddScoped<IUserRepository, SqlUserPoc>();
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
