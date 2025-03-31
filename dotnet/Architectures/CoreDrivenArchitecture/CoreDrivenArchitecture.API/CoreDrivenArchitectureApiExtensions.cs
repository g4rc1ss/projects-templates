using CoreDrivenArchitecture.Data;
using CoreDrivenArchitecture.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreDrivenArchitecture.API;

public static class CoreDrivenArchitectureApiExtensions
{
    public static void InitCoreDrivenArchitectureApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(CoreDrivenArchitectureApiExtensions).Assembly);

        builder.Services.AddUseCases();
        builder.Services.AddData();
    }
}