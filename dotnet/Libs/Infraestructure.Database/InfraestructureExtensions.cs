using Microsoft.Extensions.Hosting;
#if (UseMongodb)
using Infraestructure.Database.Entities;
using Infraestructure.Database.Repository;
using Microsoft.Extensions.DependencyInjection;
#endif

#if (SqlDatabase)
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

#if (UseIdentity)
        builder.Services.AddDbContextPool<DatabaseContext>(builder =>
#else
        builder.Services.AddDbContextPool<IdentityDatabaseContext>(builder =>
#endif

        {
#if (UsePostgres)
            builder.UseNpgsql(connectionString);
#elif (UseSqlServer)
            builder.UseSqlServer(connectionString);
#elif (UseAzureSql)
            builder.UseAzureSql(connectionString);
#elif (UseSqlite)
            builder.UseSqlite(connectionString);
#endif
        });

#if (UseIdentity)
        builder.Services.AddDbContextFactory<DatabaseContext>(builder =>
#else
        builder.Services.AddDbContextFactory<IdentityDatabaseContext>(builder =>
#endif
        {
#if (UsePostgres)
            builder.UseNpgsql(connectionString);
#elif (UseSqlServer)
            builder.UseSqlServer(connectionString);
#elif (UseAzureSql)
            builder.UseAzureSql(connectionString);
#elif (UseSqlite)
            builder.UseSqlite(connectionString);
#endif
        });


#if (UseIdentity)
        builder.Services.AddScoped<IIdentityUserRepository, IdentityUserPoc>();
#else
        builder.Services.AddScoped<IUserRepository, SqlUserPoc>();
#endif
#endif
#if (UseMongodb)
        builder.Services.AddScoped<IMongoPoc, MongoPoc>();
        builder.AddMongoDBClient("mongo");
#endif
    }
}