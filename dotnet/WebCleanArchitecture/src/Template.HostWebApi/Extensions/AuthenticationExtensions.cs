#if (UseIdentity)
using Infraestructure.Database;
using Infraestructure.Database.Entities;
using Microsoft.AspNetCore.Identity;
#endif
#if (UseIdentity || UseJwt)
using Template.HostWebApi.ConfigurationOptions;
using AuthManager.Application.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.Text;
#endif

namespace Template.HostWebApi.Extensions;

public static class AuthenticationExtensions
{
    internal static void AddAuthenticationProtocol(this IServiceCollection services,
        IConfiguration configuration)
    {
#if (UseIdentity || UseJwt)
        services
#if (UseApi)
            .AddHttpContextAccessor()
#endif
            .AddAuthorization()
            .AddAuthentication(options =>
            {
            })
            .AddJwtBearer(options =>
            {
                IConfigurationSection jwtOptionsSection = configuration
                    .GetSection("Jwt");

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = jwtOptionsSection["Issuer"],
                    ValidAudience = jwtOptionsSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptionsSection["Key"]!))
                };
            });
        services.AddScoped<IJwtTokenManagement, JwtTokenManagement>();
        services.Configure<JwtOption>(configuration.GetSection("Jwt"));
#endif

#if (UseIdentity)
        services.AddIdentityCore<UserEntity>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddUserManager<UserManager<UserEntity>>()
            .AddRoles<RoleEntity>()
            .AddRoleManager<RoleManager<RoleEntity>>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<DatabaseContext>();
#endif
    }
}