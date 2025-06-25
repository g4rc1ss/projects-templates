using Microsoft.Extensions.Hosting;
#if (UseJwt)
using System.Text;
using Infraestructure.Auth.JwtManager;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Infraestructure.Auth.JwtManager.Repository;
#endif

#if (UseIdentity)
using Infraestructure.Database.Contexts;
using Infraestructure.Database.Entities;
using Microsoft.AspNetCore.Identity;
#endif

#if (UseIdentity || UseJwt)
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Infraestructure.Auth;

public static class InfraAuthExtensions
{
    public static void AddAuthenticationProtocol(this IHostApplicationBuilder builder)
    {
#if (UseJwt)
        builder.Services.AddScoped<IJwtTokenManagement, JwtTokenManagement>();
        builder.Services.AddScoped<IJwtRepository, JwtRepository>();
        builder
            .Services.AddOptions<JwtOption>()
            .Bind(builder.Configuration.GetSection("Jwt"))
            .ValidateOnStart();

        JwtOption? jwtOptionsSection = builder.Configuration.GetSection("Jwt").Get<JwtOption>();
        ArgumentNullException.ThrowIfNull(jwtOptionsSection);

        builder
            .Services.AddHttpContextAccessor()
            .AddAuthorization()
            .AddAuthentication(options => { })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = jwtOptionsSection.Issuer,
                    ValidAudience = jwtOptionsSection.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptionsSection.Key)
                    ),
                };
            });
#endif

#if (UseIdentity)
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
            .AddEntityFrameworkStores<IdentityContext>();
#endif
    }
}
