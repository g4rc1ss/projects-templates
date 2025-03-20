using AuthManager.Application;
using AuthManager.Domain;
using AuthManager.Infraestructure;
using Infraestructure.Database.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthManager.API;

public static class AuthManagerApiExtensions
{
    public static void InitAuthManager(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(AuthManagerApiExtensions).Assembly);

        AddAuthentication(builder);
        builder.Services.AddBusinessServices();
        builder.Services.AddRepositoryService(builder.Configuration);
    }

    private static void AddAuthentication(WebApplicationBuilder builder)
    {
        builder.Services
            .AddHttpContextAccessor()
            .AddAuthorization()
            .AddAuthentication(options =>
            {
            })
            .AddJwtBearer(options =>
            {
                IConfigurationSection jwtOptionsSection = builder.Configuration
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

        builder.Services.AddIdentityCore<UserEntity>(options =>
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
            // .AddRoles<RoleEntity>()
            // .AddRoleManager<RoleManager<RoleEntity>>()
            .AddDefaultTokenProviders()
            // .AddEntityFrameworkStores<>()
            ;

        builder.Services.AddScoped<IJwtTokenManagement, JwtTokenManagement>();
        builder.Services.Configure<JwtOption>(builder.Configuration.GetSection("Jwt"));
    }
}