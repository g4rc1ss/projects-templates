using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure;

public static class InfraestructureExtensions
{
    public static void AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
    {
    }
    
    public static void AddEventsServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(InfraestructureExtensions).Assembly);
    }
}
