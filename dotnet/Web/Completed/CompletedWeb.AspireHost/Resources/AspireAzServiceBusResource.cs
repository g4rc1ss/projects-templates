using Aspire.Hosting.Azure;

namespace CompletedWeb.AspireHost.Resources;

public static class AspireAzServiceBusResource
{
    internal static IResourceBuilder<AzureServiceBusQueueResource> AddAspireAzureServiceBus(
        this IDistributedApplicationBuilder builder
    )
    {
        return builder
            .AddAzureServiceBus("ServiceBus")
            .RunAsEmulator()
            .AddServiceBusQueue("AzureServiceBus", "ServiceBusQueue");
    }

    internal static IResourceBuilder<T> WithAspireAzServiceBus<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<AzureServiceBusQueueResource> azureServiceBus
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(azureServiceBus).WaitFor(azureServiceBus);
    }
}
