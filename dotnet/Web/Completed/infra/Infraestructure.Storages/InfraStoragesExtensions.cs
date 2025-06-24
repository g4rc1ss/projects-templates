using Microsoft.Extensions.Hosting;
#if (UseLocalStorage || UseAzureBlobStorage)
using Microsoft.Extensions.DependencyInjection;
#endif
#if (UseAzureBlobStorage)
using Azure.Identity;
using Infraestructure.Storages.AzureStorage;
using Microsoft.Extensions.Configuration;
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
        if (!storageSettings.DisableTracing)
        {
            builder.ConfigureOpenTelemetry();
        }

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

        string? blobStorage = builder.Configuration.GetConnectionString("AzureBlobStorage");
        if (!string.IsNullOrEmpty(blobStorage))
        {
            builder.AddAzureBlobClient(
                "AzureBlobStorage",
                settings =>
                {
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
