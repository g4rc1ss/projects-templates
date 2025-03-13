using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Functionality.Infraestructure;

public static class FunctionalityInfraestructureExtensions
{
    public static void AddDataAccessService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositoryServices();
    }

    private static void AddRepositoryServices(this IServiceCollection services)
    {
    }
}
