using Functionality.Application;
using Functionality.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Functionality.API;

public static class FunctionalityApiExtensions
{
    public static void InitFunctionality(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBusinessServices();
        services.AddDataAccessService(configuration);
    }
}