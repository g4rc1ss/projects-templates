using MongoDB.Bson.Serialization.Attributes;

namespace SimpleWeb.HostWebApi.Database.Entities;

public class MongoDbEntity
{
    [BsonId]
    public required string Id { get; set; }

    public string? Property { get; set; }
}
