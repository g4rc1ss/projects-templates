using Infraestructure.Database.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if (UseMongodb)
using Infraestructure.Database.Entities;
#endif

#if (SqlDatabase || UseIdentity)
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
#endif

namespace Infraestructure.Database;

public static class InfraestructureDatabaseExtensions
{
    public static void AddDatabaseConfig(this IHostApplicationBuilder builder)
    {
#if (SqlDatabase || UseIdentity)

#if (UseIdentity)
        string? connectionString = builder.Configuration.GetConnectionString(
            nameof(IdentityDatabaseContext)
        );
#else
        string? connectionString = builder.Configuration.GetConnectionString(
            nameof(DatabaseContext)
        );
#endif
        ArgumentNullException.ThrowIfNull(connectionString);

#if (UseIdentity)
        builder.Services.AddDbContextPool<IdentityDatabaseContext>(dbContextBuilder =>
#else
        builder.Services.AddDbContextPool<DatabaseContext>(dbContextBuilder =>
#endif

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

#if (UseIdentity)
        builder.Services.AddDbContextFactory<IdentityDatabaseContext>(dbContextBuilder =>
#else
        builder.Services.AddDbContextFactory<DatabaseContext>(dbContextBuilder =>
#endif
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
