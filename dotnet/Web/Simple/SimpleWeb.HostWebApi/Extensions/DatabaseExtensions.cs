using SimpleWeb.HostWebApi.Database.Repository;
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
            dbContextBuilder.UseNpgsql(connectionString);
        });

        builder.Services.AddDbContextFactory<DatabaseContext>(dbContextBuilder =>
        {
            dbContextBuilder.UseNpgsql(connectionString);
        });

        builder.Services.AddScoped<IUserRepository, SqlUserPoc>();
#endif
        builder.Services.AddScoped<IMongoPoc, MongoPoc>();
        builder.AddMongoDBClient("mongo");
    }
}
