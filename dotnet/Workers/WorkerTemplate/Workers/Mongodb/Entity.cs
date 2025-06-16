using MongoDB.Bson.Serialization.Attributes;

namespace WorkerTemplate.Workers.Mongodb;

public class Entity
{
    [BsonId]
    public required string Id { get; set; }

    public string? Property { get; set; }
}
