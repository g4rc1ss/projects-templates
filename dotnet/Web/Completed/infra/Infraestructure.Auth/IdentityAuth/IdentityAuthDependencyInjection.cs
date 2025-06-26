using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infraestructure.Auth.IdentityAuth;

public static class IdentityAuthDependencyInjection
{
    public static void AddIdentityAuth(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddIdentityApiEndpoints<IdentityUserEntity>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddUserManager<UserManager<IdentityUserEntity>>()
            .AddRoles<IdentityRoleEntity>()
            .AddRoleManager<RoleManager<IdentityRoleEntity>>()
            .AddEntityFrameworkStores<IdentityAuthContext>();
    }
}
