using Microsoft.Extensions.Hosting;
#if (UseLocalStorage)
using Microsoft.Extensions.DependencyInjection;
#endif

#if (UseAzureBlobStorage)
using Azure.Identity;
using Infraestructure.Storages.AzureStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Infraestructure.Storages;

public static class InfraStoragesExtensions
{
    internal const string STORAGE_TRACE = "Storage.Tracing";

    public static void AddStorages(
        this IHostApplicationBuilder builder,
        Action<StorageSettings>? configureStorageSettings = null
    )
    {
        StorageSettings storageSettings = new();
        configureStorageSettings?.Invoke(storageSettings);
#if (UseAzureBlobStorage)
        builder.AddAzureBlobStorage(storageSettings);
#endif
#if (UseLocalStorage)
        builder.ConfigureOpenTelemetry();
        builder.Services.AddSingleton<IFileStorage, LocalStorage>();
#endif
    }

#if (UseAzureBlobStorage)
    private static void AddAzureBlobStorage(
        this IHostApplicationBuilder builder,
        StorageSettings storageSettings
    )
    {
        builder
            .Services.AddOptions<AzureBlobStorageOptions>()
            .Bind(builder.Configuration.GetSection("AzureBlobStorage"))
            .ValidateOnStart();

        builder.Services.AddSingleton<IFileStorage, AzureBlobStorage>();

        AzureBlobStorageOptions? blobStorage = builder
            .Configuration.GetSection("AzureBlobStorage")
            .Get<AzureBlobStorageOptions>();

        if (!string.IsNullOrEmpty(blobStorage?.AccountName))
        {
            builder.AddAzureBlobClient(
                "",
                settings =>
                {
                    settings.ConnectionString =
                        $"https://{blobStorage.AccountName}.blob.core.windows.net";
                    settings.DisableTracing = storageSettings.DisableTracing;
                    settings.Credential = new DefaultAzureCredential();
                }
            );
        }
    }
#endif

#if (UseLocalStorage)
    private static void ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddOpenTelemetry()
            .WithTracing(providerBuilder => providerBuilder.AddSource(STORAGE_TRACE));
    }
#endif
}
