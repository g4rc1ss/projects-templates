#if (UseAzureKeyVault)
using CompletedWeb.HostWebApi.ConfigurationOptions.AzureOptions;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
#endif

namespace CompletedWeb.HostWebApi.Extensions;

public static class KeyVaultExtensions
{
    public static void AddKeyVault(this IHostApplicationBuilder hostBuilder)
    {
#if (UseAzureKeyVault)
        AddAzureKeyVault(hostBuilder);
#endif
    }

#if (UseAzureKeyVault)
    private static void AddAzureKeyVault(IHostApplicationBuilder builder)
    {
        builder
            .Services.AddOptions<AzureKeyVaultOptions>()
            .Bind(builder.Configuration.GetSection("KeyVault"))
            .ValidateOnStart();

        AzureKeyVaultOptions? keyVaultConfig = builder
            .Configuration.GetSection("KeyVault")
            .Get<AzureKeyVaultOptions>();
        if (!string.IsNullOrEmpty(keyVaultConfig?.AccountName))
        {
            builder.Configuration.AddAzureKeyVault(
                new Uri($"https://{keyVaultConfig.AccountName}.vault.azure.net/"),
                new DefaultAzureCredential(),
                new AzureKeyVaultConfigurationOptions { ReloadInterval = TimeSpan.FromHours(1) }
            );
        }
    }
#endif
}
