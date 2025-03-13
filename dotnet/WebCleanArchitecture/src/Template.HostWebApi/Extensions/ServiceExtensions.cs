using Infraestructure.Database;
using Infraestructure.Events;

namespace Template.HostWebApi.Extensions;

internal static class ServiceExtensions
{
    internal static void InitTemplateHostConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
        builder.Services.AddOptions();

        builder.Services.AddHttpClient();

        builder.Services.ConfigureDataProtectionProvider(builder.Configuration);

        builder.Services.AddDatabaseConfig(builder.Configuration);
        builder.Services.AddEventsServices(builder.Configuration);

        InitializeFunctionalities(builder.Services, builder.Configuration);
    }

    private static void InitializeFunctionalities(this IServiceCollection services, IConfiguration configuration)
    {
        // services.Init(builder.Configuration);
    }
}