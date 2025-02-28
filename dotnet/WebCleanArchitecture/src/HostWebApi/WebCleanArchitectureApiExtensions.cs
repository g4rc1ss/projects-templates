using Microsoft.AspNetCore.DataProtection;
using WebCleanArchitecture.UsesCases;
using WebCleanArchitecture.Infraestructure;

namespace HostWebApi;

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
