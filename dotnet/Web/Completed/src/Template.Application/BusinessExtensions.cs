using Microsoft.Extensions.DependencyInjection;
using Template.Application.UsesCases;

namespace Template.Application;

public static class BusinessExtensions
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IExample, Example>();
    }
}
