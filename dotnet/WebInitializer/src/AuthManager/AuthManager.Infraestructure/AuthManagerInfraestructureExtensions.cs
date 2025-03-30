using AuthManager.Application.Contracts.InfraestructureContracts;
using AuthManager.Infraestructure.Repositories.Identity;
using Infraestructure.Database.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthManager.Infraestructure;

public static class AuthManagerInfraestructureExtensions
{
    public static void AddRepositoryService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IPasswordManager, PasswordManager>();
        services.AddScoped<IEmailManager, EmailManager>();
        services.AddScoped<IUserInfo, UserInfo>();
        services.AddScoped<ITotpManager, TotpManager>();
        services.AddScoped<IRoleManager, RoleManager>();
    }
}