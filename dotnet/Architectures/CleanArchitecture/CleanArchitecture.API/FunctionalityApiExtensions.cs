using CleanArchitecture.Application;
using CleanArchitecture.Infraestructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanArchitecture.API;

public static class FunctionalityApiExtensions
{
    public static void InitFunctionalityApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(FunctionalityApiExtensions).Assembly);

        builder.Services.AddBusinessServices();
        builder.AddDataAccessService();
    }
}