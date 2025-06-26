using System.Text;
using Infraestructure.Auth.JwtManager.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Infraestructure.Auth.JwtManager;

public static class JwtDependencyInjection
{
    public static void AddJwt(this IHostApplicationBuilder builder)
    {
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
    }
}
