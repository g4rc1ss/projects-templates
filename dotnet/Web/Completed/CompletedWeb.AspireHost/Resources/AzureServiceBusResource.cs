using Aspire.Hosting.Azure;

namespace CompletedWeb.AspireHost.Resources;

public static class AzServiceBusResource
{
    internal static IResourceBuilder<AzureServiceBusResource> AddAspireAzureServiceBus(
        this IDistributedApplicationBuilder builder
    )
    {
        return builder.AddAzureServiceBus("AzureServiceBus").RunAsEmulator();
    }

    internal static IResourceBuilder<T> WithAspireAzServiceBus<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<AzureServiceBusResource> azureServiceBus
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(azureServiceBus).WaitFor(azureServiceBus);
    }
}
