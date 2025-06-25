namespace CompletedWeb.AspireHost.Resources;

public static class AspireAzureCosmosResource
{
    internal static IResourceBuilder<AzureCosmosDBResource> AddAspireAzureCosmos(
        this IDistributedApplicationBuilder builder
    )
    {
        IResourceBuilder<AzureCosmosDBResource> cosmosdb = builder
            .AddAzureCosmosDB("CosmosDatabase")
            .RunAsEmulator();

        cosmosdb
            .AddCosmosDatabase("DatabaseResource", "Database")
            .AddContainer("ContainerResource", "/id", "Container");

        return cosmosdb;
    }

    internal static IResourceBuilder<T> WithAspireAzureCosmos<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<AzureCosmosDBResource> cosmosdb
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(cosmosdb).WaitFor(cosmosdb);
    }
}
