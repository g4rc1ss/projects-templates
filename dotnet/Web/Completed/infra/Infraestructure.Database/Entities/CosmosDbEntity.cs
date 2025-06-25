using Newtonsoft.Json;

namespace Infraestructure.Database.Entities;

public class CosmosDbEntity
{
    [JsonProperty(PropertyName = "id")]
    public required string Id { get; set; }

    public string? Property { get; set; }
}
