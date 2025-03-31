using NLayerArchitecture.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLayerArchitecture.Infraestructure;

namespace NLayerArchitecture.API;

public static class NLayerArchitectureApiExtensions
{
    public static void InitN_LayerArchitectureApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(NLayerArchitectureApiExtensions).Assembly);

        builder.Services.AddBusinessServices();
        builder.AddDataAccessService();
    }
}