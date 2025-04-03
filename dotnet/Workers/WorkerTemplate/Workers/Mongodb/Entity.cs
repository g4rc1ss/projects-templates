using MongoDB.Bson.Serialization.Attributes;

namespace WorkerTemplate.Workers.Mongodb;

public class Entity
{
    [BsonId]
    public string Id { get; set; }

    public string Property { get; set; }
}