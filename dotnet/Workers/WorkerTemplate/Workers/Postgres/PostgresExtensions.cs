using Microsoft.EntityFrameworkCore;

namespace WorkerTemplate.Workers.Postgres;

public static class PostgresExtensions
{
    internal static void AddPostgres(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<PostgresDbContext>("DatabaseContext");
        builder.Services.AddPooledDbContextFactory<PostgresDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseContext")));

        builder.Services.AddHostedService<MigrateAndSeed>();
        builder.Services.AddHostedService<PostgresWorker>();
    }
}