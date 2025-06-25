using Aspire.Hosting.Azure;

namespace CompletedWeb.AspireHost.Resources;

public static class AzureCosmosResource
{
    internal static IResourceBuilder<AzureCosmosDBDatabaseResource> AddAspireAzureCosmos(
        this IDistributedApplicationBuilder builder
    )
    {
        return builder
            .AddAzureCosmosDB("Cosmos")
            .RunAsEmulator()
            .AddCosmosDatabase("CosmosDatabase");
    }

    internal static IResourceBuilder<T> WithAspireAzureCosmos<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<AzureCosmosDBDatabaseResource> cosmosdb
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(cosmosdb).WaitFor(cosmosdb);
    }
}
