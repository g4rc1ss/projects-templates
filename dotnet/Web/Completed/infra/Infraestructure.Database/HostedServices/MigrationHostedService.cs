using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if (SqlDatabase)
using Infraestructure.Database.Contexts;
using Microsoft.EntityFrameworkCore;
#endif

namespace Infraestructure.Database.HostedServices;

public class MigrationHostedService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
#if (UsePostgres)
        PostgresContext postgresContext =
            scope.ServiceProvider.GetRequiredService<PostgresContext>();
        await postgresContext.Database.MigrateAsync(cancellationToken: cancellationToken);

#endif
#if (UseSqlite)
        SqliteContext sqliteContext = scope.ServiceProvider.GetRequiredService<SqliteContext>();
        await sqliteContext.Database.MigrateAsync(cancellationToken: cancellationToken);

#endif
#if (UseSqlServer)
        SqlServerContext sqlServerContext =
            scope.ServiceProvider.GetRequiredService<SqlServerContext>();
        await sqlServerContext.Database.MigrateAsync(cancellationToken: cancellationToken);
#endif
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
