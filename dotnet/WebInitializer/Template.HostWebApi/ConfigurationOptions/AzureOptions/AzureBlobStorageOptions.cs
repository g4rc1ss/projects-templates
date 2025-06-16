namespace Template.HostWebApi.ConfigurationOptions.AzureOptions;

public record AzureBlobStorageOptions
{
    public required string AccountName { get; init; }
}
