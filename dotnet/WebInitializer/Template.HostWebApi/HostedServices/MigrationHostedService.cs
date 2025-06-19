using Infraestructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Template.HostWebApi.HostedServices;

public class MigrationHostedService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
#if (UseIdentity)
        IdentityDatabaseContext context = scope.ServiceProvider.GetRequiredService<IdentityDatabaseContext>();
#else
        DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
#endif
        await context.Database.MigrateAsync(cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}