using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcArchitecture.API;

public static class ApiExtensions
{
    public static void InitMvcArchitectureApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddApplicationPart(typeof(ApiExtensions).Assembly);
    }
}
