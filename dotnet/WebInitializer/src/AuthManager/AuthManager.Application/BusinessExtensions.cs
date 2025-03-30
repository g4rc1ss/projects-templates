using AuthManager.Application.UsesCases.CreateUserCase;
using AuthManager.Application.UsesCases.LoginCase;
using Microsoft.Extensions.DependencyInjection;

namespace AuthManager.Application;

public static class BusinessExtensions
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
    }
}