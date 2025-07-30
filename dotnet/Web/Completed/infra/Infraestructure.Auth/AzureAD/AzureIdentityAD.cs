using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;

namespace Infraestructure.Auth.AzureAD;

public static class AzureIdentityAd
{
    public static void AddAzureAd(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddOptions<AzureAdOptions>()
            .Bind(builder.Configuration.GetSection("AzureAd"))
            .ValidateOnStart();

        AzureAdOptions? azureAdOptions = builder.Configuration.GetSection("AzureAd").Get<AzureAdOptions>();

        AuthenticationBuilder auth = builder
            .Services.AddAuthorization()
            .AddAuthentication();

        if (string.IsNullOrEmpty(azureAdOptions?.ClientId))
        {
            return;
        }

        auth.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
    }
}