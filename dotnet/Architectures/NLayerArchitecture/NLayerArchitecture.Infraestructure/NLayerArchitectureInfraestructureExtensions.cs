using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NLayerArchitecture.Infraestructure;

public static class NLayerArchitectureInfraestructureExtensions
{
    public static void AddDataAccessService(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRepositoryServices();
    }

    private static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<Repository>();
    }
}