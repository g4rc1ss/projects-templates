namespace Infraestructure.Storages.AzureStorage;

public record AzureBlobStorageOptions
{
    public required string AccountName { get; init; }
}
