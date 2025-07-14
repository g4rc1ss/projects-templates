using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infraestructure.Auth.ApiKey;

public static class ApiKeyDependencyInjection
{
    public static void AddApiKey(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddAuthorization()
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Constants.API_KEY_SCHEME;
                options.DefaultChallengeScheme = Constants.API_KEY_SCHEME;
            })
            .AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>(
                Constants.API_KEY_SCHEME,
                options => { }
            );
    }
}
