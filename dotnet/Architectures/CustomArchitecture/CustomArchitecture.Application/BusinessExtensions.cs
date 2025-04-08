using CustomArchitecture.Application.UsesCases;
using Microsoft.Extensions.DependencyInjection;

namespace CustomArchitecture.Application;

public static class BusinessExtensions
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IExample, Example>();
    }
}