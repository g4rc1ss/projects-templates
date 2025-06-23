#if (SqlDatabase || NoSqlDatabase)
using SimpleWeb.HostWebApi.Database.Repository;
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
        });

        builder.Services.AddDbContextFactory<DatabaseContext>(dbContextBuilder =>
        {
#if (UsePostgres)
            dbContextBuilder.UseNpgsql(connectionString);
#endif
        });

        builder.Services.AddScoped<IUserRepository, SqlUserPoc>();
#endif
#if (UseMongodb)
        builder.Services.AddScoped<IMongoPoc, MongoPoc>();
        builder.AddMongoDBClient("mongo");
#endif
    }
}
