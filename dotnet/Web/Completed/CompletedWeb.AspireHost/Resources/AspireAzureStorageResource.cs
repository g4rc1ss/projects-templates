using Aspire.Hosting.Azure;

namespace CompletedWeb.AspireHost.Resources;

public static class AspireAzureStorageResource
{
    internal static IResourceBuilder<AzureStorageResource> AddAspireAzBlobStorage(
        this IDistributedApplicationBuilder builder
    )
    {
        IResourceBuilder<Aspire.Hosting.Azure.AzureStorageResource> blobStorage = builder
            .AddAzureStorage("AzureBlobStorage")
            .RunAsEmulator();

        blobStorage
            .AddBlobs("AzureBlobs")
            .AddBlobContainer("AzureBlobContainer", "blob");

        return blobStorage;
    }

    internal static IResourceBuilder<T> WithAspireAzBlobStorage<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<AzureBlobStorageContainerResource> blobStorage
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(blobStorage).WaitFor(blobStorage);
    }
}