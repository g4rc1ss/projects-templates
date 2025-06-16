namespace Template.HostWebApi.ConfigurationOptions.AzureOptions;

public record AzureKeyVaultOptions
{
    public required string AccountName { get; init; }
}
