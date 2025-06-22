using Microsoft.Extensions.Hosting;
#if (UseIdentity)
using Infraestructure.Auth.HostedServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#endif


namespace Infraestructure.Auth;

public static class AuthExtensions
{
    public static void AddAuthConfig(this IHostApplicationBuilder builder)
    {
#if (UseIdentity)
        builder.Services.AddSingleton<MigrationHostedService>();
        builder.Services.AddHostedService<MigrationHostedService>();

        string? connectionString = builder.Configuration.GetConnectionString(
            nameof(IdentityDatabaseContext)
        );
        ArgumentNullException.ThrowIfNull(connectionString);

        builder.Services.AddDbContextPool<IdentityDatabaseContext>(dbContextBuilder =>
        {
            dbContextBuilder.UseNpgsql(connectionString);
        });

        builder.Services.AddDbContextFactory<IdentityDatabaseContext>(dbContextBuilder =>
        {
            dbContextBuilder.UseNpgsql(connectionString);
        });
#endif
    }
}
