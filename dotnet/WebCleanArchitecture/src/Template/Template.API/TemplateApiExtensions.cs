using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Infraestructure;
using Template.UsesCases;

namespace Template.API;

public static class TemplateApiExtensions
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
