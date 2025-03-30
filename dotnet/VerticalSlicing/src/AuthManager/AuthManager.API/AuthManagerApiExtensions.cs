using AuthManager.Application;
using AuthManager.Infraestructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AuthManager.API;

public static class AuthManagerApiExtensions
{
    public static void InitAuthManager(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(AuthManagerApiExtensions).Assembly);

        builder.Services.AddBusinessServices();
        builder.Services.AddRepositoryService(builder.Configuration);
    }
}