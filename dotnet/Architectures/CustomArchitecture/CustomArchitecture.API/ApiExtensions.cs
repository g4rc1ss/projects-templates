using CustomArchitecture.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CustomArchitecture.API;

public static class ApiExtensions
{
    public static void InitCustomArchitectureApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddApplicationPart(typeof(ApiExtensions).Assembly);

        builder.Services.AddBusinessServices();
    }
}
