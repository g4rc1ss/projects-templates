namespace WorkerTemplate.Workers.Mongodb;

public static class MongodbExtensions
{
    internal static void AddMongodb(this IHostApplicationBuilder builder)
    {
        builder.AddMongoDBClient("mongo");
        builder.Services.AddHostedService<MongoDatabaseWorker>();
    }
}