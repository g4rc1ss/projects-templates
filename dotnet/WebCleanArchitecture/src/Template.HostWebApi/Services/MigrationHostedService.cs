using Infraestructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Template.HostWebApi.Services;

public class MigrationHostedService(
    DatabaseContext context
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await context.Database.MigrateAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}