using System.Text.Json.Serialization;

namespace Infraestructure.Database.Entities;

public class CosmosDbEntity
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    public string? Property { get; set; }
}
