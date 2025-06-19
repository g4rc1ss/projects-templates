using Microsoft.Extensions.Hosting;
#if (UseLocalStorage)
using Microsoft.Extensions.DependencyInjection;
#endif

#if (UseAzureBlobStorage)
using Azure.Identity;
using Azure.Storage.Blobs;
using Infraestructure.Storages.AzureStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Infraestructure.Storages;

public static class StoragesExtensions
{
    public static void AddStorages(this IHostApplicationBuilder builder)
    {
#if (UseAzureBlobStorage)
        builder.AddAzureBlobStorage();
#endif
#if (UseLocalStorage)
        builder.Services.AddSingleton<IFileStorage, LocalStorage>();
#endif
    }

#if (UseAzureBlobStorage)
    private static void AddAzureBlobStorage(this IHostApplicationBuilder builder)
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
}
