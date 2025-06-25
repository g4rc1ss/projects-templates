using Aspire.Hosting.Azure;

namespace CompletedWeb.AspireHost.Resources;

public static class AspireAzServiceBusResource
{
    internal static IResourceBuilder<AzureServiceBusResource> AddAspireAzureServiceBus(
        this IDistributedApplicationBuilder builder
    )
    {
        IResourceBuilder<AzureServiceBusResource> azServiceBus = builder
            .AddAzureServiceBus("AzureServiceBus")
            .RunAsEmulator();

        azServiceBus
            .AddServiceBusQueue("ServiceBusQueue", "ServiceBusQueue");

        return azServiceBus;
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