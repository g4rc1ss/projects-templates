using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infraestructure.AuthManagerDB;

public static class InfraestructureDatabaseExtensions
{
    public static void AddDatabaseConfig(this IHostApplicationBuilder builder)
    {
        string? connectionString = builder.Configuration.GetConnectionString("RedNeuronalSqlDatabase");
        ArgumentNullException.ThrowIfNull(connectionString);

        builder.Services.AddDbContextPool<DbContext>(builder =>
            builder.UseNpgsql(connectionString)
        );

        builder.Services.AddDbContextFactory<DbContext>(builder =>
            builder.UseNpgsql(connectionString)
        );
    }
}