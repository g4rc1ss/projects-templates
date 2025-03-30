using CleanArchitecture.Application.UsesCases;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class BusinessExtensions
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IExample, Example>();
    }
}