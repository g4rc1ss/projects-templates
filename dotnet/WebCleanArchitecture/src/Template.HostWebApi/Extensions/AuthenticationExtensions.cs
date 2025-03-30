#if (UseCustomIdentity)
using Infraestructure.Database;
using Infraestructure.Database.Entities;
using Microsoft.AspNetCore.Identity;
#endif
#if (UseCustomIdentity || UseJwt)
using Microsoft.IdentityModel.Tokens;
using Shared.JWT;
using System.Text;
using Template.HostWebApi.ConfigurationOptions;
using Template.HostWebApi.JwtManagement;
#endif

namespace Template.HostWebApi.Extensions;

public static class AuthenticationExtensions
{
    internal static void AddAuthenticationProtocol(this IServiceCollection services,
        IConfiguration configuration)
    {
#if (UseCustomIdentity || UseJwt)
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
        services.AddScoped<IJwtRepository, JwtRepository>();
        services.Configure<JwtOption>(configuration.GetSection("Jwt"));
#endif

#if (UseCustomIdentity)
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