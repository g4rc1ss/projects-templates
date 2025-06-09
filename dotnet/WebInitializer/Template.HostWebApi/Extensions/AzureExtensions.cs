#if (UseAzureOpts)
using Template.HostWebApi.ConfigurationOptions.AzureOptions;
using Azure.Identity;
#endif

#if (UseAzureBlobStorage)
using Azure.Storage.Blobs;
using Infraestructure.Storages;
#endif
#if (UseAzureKeyVault)
using Azure.Extensions.AspNetCore.Configuration.Secrets;
#endif

namespace Template.HostWebApi.Extensions;

public static class AzureExtensions
{
    public static void AddAzureServices(this IHostApplicationBuilder hostBuilder)
    {
#if (UseAzureKeyVault)
        AddAzureKeyVault(hostBuilder);
#endif
#if (UseAzureBlobStorage)
        AddAzureBlobStorage(hostBuilder);
#endif
    }

#if (UseAzureBlobStorage)
    private static void AddAzureBlobStorage(IHostApplicationBuilder builder)
    {
        builder
            .Services.AddOptions<AzureBlobStorageOptions>()
            .Bind(builder.Configuration.GetSection("AzureBlobStorage"))
            .ValidateOnStart();

        builder.Services.AddSingleton<IFileStorage, AzureBlobStorage>();

        AzureBlobStorageOptions? blobStorage = builder.Configuration.Get<AzureBlobStorageOptions>();
        if (!string.IsNullOrEmpty(blobStorage?.AccountName))
        {
            builder.Services.AddSingleton<BlobServiceClient>(services => new BlobServiceClient(
                new Uri($"https://{blobStorage.AccountName}.blob.core.windows.net"),
                new DefaultAzureCredential(),
                new BlobClientOptions() { }
            ));
        }
    }
#endif

#if (UseAzureKeyVault)
    private static void AddAzureKeyVault(IHostApplicationBuilder builder)
    {
        builder
            .Services.AddOptions<AzureKeyVaultOptions>()
            .Bind(builder.Configuration.GetSection("KeyVault"))
            .ValidateOnStart();

        AzureKeyVaultOptions? keyVaultConfig = builder.Configuration.Get<AzureKeyVaultOptions>();
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
