using Microsoft.Extensions.DependencyInjection;
using NLayerArchitecture.Application.UsesCases;

namespace NLayerArchitecture.Application;

public static class BusinessExtensions
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IExample, Example>();
    }
}