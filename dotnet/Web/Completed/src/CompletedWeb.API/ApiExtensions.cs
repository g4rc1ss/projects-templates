using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CompletedWeb.Application;

namespace CompletedWeb.API;

public static class ApiExtensions
{
    public static void InitTemplateApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddApplicationPart(typeof(ApiExtensions).Assembly);

        builder.Services.AddBusinessServices();
    }
}
