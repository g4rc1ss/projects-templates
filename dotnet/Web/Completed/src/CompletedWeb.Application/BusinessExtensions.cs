using System.Text;
using CompletedWeb.Application.UsesCases;
using Microsoft.Extensions.DependencyInjection;

namespace CompletedWeb.Application;

public static class BusinessExtensions
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
        StringBuilder x = new();
        services.AddScoped<IExample, Example>();
    }
}
