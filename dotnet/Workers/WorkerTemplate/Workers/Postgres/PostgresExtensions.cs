namespace WorkerTemplate.Workers.Postgres;

public static class PostgresExtensions
{
    internal static void AddPostgres(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<PostgresDbContext>("Postgres");
        builder.Services.AddHostedService<PostgresWorker>();
    }
}