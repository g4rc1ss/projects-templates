#if (UseIdentity)
using Infraestructure.Database;
using Infraestructure.Database.Entities;
using Microsoft.AspNetCore.Identity;
#endif

namespace Template.HostWebApi.Extensions;

public static class AuthenticationExtensions
{
    internal static void AddAuthenticationProtocol(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {

#if (UseIdentity)
        services
            .AddIdentityApiEndpoints<IdentityUserEntity>(options =>
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
            .AddEntityFrameworkStores<IdentityDatabaseContext>();
#endif
    }
}
