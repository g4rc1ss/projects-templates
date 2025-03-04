using Infraestructure.Database;
using Infraestructure.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Infraestructure;

public static class TemplateInfraestructureExtensions
{
    public static void AddDataAccessService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfig(configuration);
        services.AddEventsServices();
        services.AddRepositoryServices();
    }

    private static void AddRepositoryServices(this IServiceCollection services)
    {
    }
}
