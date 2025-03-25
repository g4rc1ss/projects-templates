using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Functionality.Infraestructure;

public static class FunctionalityInfraestructureExtensions
{
    public static void AddDataAccessService(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRepositoryServices();
    }

    private static void AddRepositoryServices(this IServiceCollection services)
    {
    }
}