using Aspire.Hosting.Azure;

namespace CompletedWeb.AspireHost.Resources;

public static class AspireAzureStorageResource
{
    internal static IResourceBuilder<AzureBlobStorageResource> AddAspireAzBlobStorage(
        this IDistributedApplicationBuilder builder
    )
    {
        IResourceBuilder<AzureBlobStorageResource> blobStorage = builder
            .AddAzureStorage("AzureBlobStorage")
            .RunAsEmulator()
            .AddBlobs("AzureBlobs");

        blobStorage
            .AddBlobContainer("AzureBlobContainer", "blob");

        return blobStorage;
    }

    internal static IResourceBuilder<T> WithAspireAzBlobStorage<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<AzureBlobStorageResource> blobStorage
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(blobStorage).WaitFor(blobStorage);
    }
}