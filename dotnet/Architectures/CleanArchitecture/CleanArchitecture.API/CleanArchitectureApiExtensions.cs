using CleanArchitecture.Application;
using CleanArchitecture.Infraestructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.API;

public static class CleanArchitectureApiExtensions
{
    public static void InitCleanArchitectureApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(CleanArchitectureApiExtensions).Assembly);

        builder.Services.AddBusinessServices();
        builder.AddDataAccessService();
    }
}