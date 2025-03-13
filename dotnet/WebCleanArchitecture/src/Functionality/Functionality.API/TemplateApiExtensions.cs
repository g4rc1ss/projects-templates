using Functionality.Application;
using Functionality.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Functionality.API;

public static class TemplateApiExtensions
{
    public static void InitTemplate(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBusinessServices();
        services.AddDataAccessService(configuration);
    }
}
