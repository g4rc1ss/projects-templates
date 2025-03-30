using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.Infraestructure;

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