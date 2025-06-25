using Aspire.Hosting.Azure;

namespace CompletedWeb.AspireHost.Resources;

public static class AzureStorageResource
{
    internal static IResourceBuilder<AzureBlobStorageResource> AddAspireAzBlobStorage(
        this IDistributedApplicationBuilder builder
    )
    {
        return builder.AddAzureStorage("AzureStorage").RunAsEmulator().AddBlobs("AzureBlobStorage");
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
