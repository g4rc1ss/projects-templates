using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Infraestructure;
using Template.Application;

namespace Template.API;

public static class TemplateApiExtensions
{
    public static void InitTemplate(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBusinessServices();
        services.AddDataAccessService(configuration);
    }
}
