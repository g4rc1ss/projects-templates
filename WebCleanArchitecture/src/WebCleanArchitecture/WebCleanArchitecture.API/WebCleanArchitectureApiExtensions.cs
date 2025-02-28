using WebCleanArchitecture.Application;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCleanArchitecture.Infraestructure;

namespace WebCleanArchitecture.API;

public static class WebCleanArchitectureApiExtensions
{
    public static void InicializarConfiguracionApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.ConfigureDataProtectionProvider(configuration);


        services.AddBusinessServices();
        services.AddDataAccessService(configuration);
    }

    private static void ConfigureDataProtectionProvider(this IServiceCollection services,
        IConfiguration configuration)
    {
        // string? keysFolder = configuration["keysFolder"]!;
        services.AddDataProtection()
            // .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
            .SetApplicationName("Aplicacion.WebApi");
    }
}
