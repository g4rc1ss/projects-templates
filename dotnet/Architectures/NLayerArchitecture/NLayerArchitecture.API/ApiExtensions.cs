using NLayerArchitecture.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLayerArchitecture.Infraestructure;

namespace NLayerArchitecture.API;

public static class ApiExtensions
{
    public static void InitNLayerArchitectureApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(ApiExtensions).Assembly);

        builder.Services.AddBusinessServices();
        builder.AddDataAccessService();
    }
}