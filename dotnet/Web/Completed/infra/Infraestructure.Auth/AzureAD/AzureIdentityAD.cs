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

        builder
            .Services.AddAuthorization()
            .AddAuthentication()
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
    }
}
