namespace Infraestructure.Auth.AzureAD;

public record AzureAdOptions
{
    public required string Audience { get; init; }
    public required string ClientId { get; init; }
    public required string TenantId { get; init; }
    public required string Instance { get; init; }
    public string? Scope { get; set; }
}
