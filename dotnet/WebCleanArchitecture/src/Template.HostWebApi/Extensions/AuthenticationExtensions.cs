using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.ConfigurationOptions;
using System.Text;

namespace Template.HostWebApi.Extensions;

public static class AuthenticationExtensions
{
    internal static void AddAuthenticationProtocol(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

        services.AddSingleton<IJwtTokenManagement, JwtTokenManagement>();
        services.Configure<JwtOption>(configuration.GetSection("Jwt"));
    }
}