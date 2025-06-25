#if (SqlDatabase)
using Microsoft.EntityFrameworkCore;
#endif

namespace SimpleWeb.HostWebApi.Database.HostedServices;

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
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
