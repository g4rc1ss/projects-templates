#if (SqlDatabase)
using SimpleWeb.HostWebApi.Database.Entities;
using SimpleWeb.HostWebApi.Database.Repository;
#endif

namespace SimpleWeb.HostWebApi.UsesCases;

public class Example(
#if (UsePostgres)
    IPostgresPoc postgresPoc,
#endif
#if (UseSqlite)
    ISqlitePoc sqlitePoc,
#endif
#if (UseMongodb)
    IMongoPoc mongoPoc,
#endif
#if (UseLitedb)
    ILitedbPoc litedbPoc,
#endif
    ILogger<Example> logger)
{
    public async Task ExecuteAsync()
    {
        Log.ExecutingTemplateLog(logger);
#if (UseSqlite)
        await SqliteAsync();
#endif
#if (UsePostgres)
        await PostgresAsync();
#endif
#if (UseMongodb)
        await MongodbAsync();
#endif
#if (UseLitedb)
        await LitedbAsync();
#endif
    }

#if (UseSqlite)
    public async Task SqliteAsync()
    {
        WeatherForecastEntity weather = new() { Name = "Nombre" };
        WeatherForecastEntity weatherCreated = await sqlitePoc.CreateAsync(weather);
        weatherCreated.Name = "Nombre Modificado";

        WeatherForecastEntity? result = await sqlitePoc.GetByIdAsync(weatherCreated.Id);
    }
#endif

#if (UsePostgres)
    public async Task PostgresAsync()
    {
        WeatherForecastEntity weather = new() { Name = "Nombre" };
        WeatherForecastEntity weatherCreated = await postgresPoc.CreateAsync(weather);
        weatherCreated.Name = "Nombre Modificado";

        WeatherForecastEntity? result = await postgresPoc.GetByIdAsync(weatherCreated.Id);
    }
#endif

#if (UseMongodb)
    public async Task MongodbAsync()
    {
        MongoDbEntity entity = new() { Id = Guid.NewGuid().ToString(), Property = "Creado" };
        MongoDbEntity weatherCreated = await mongoPoc.CreateAsync(entity);
        weatherCreated.Property = "Modificado";
        MongoDbEntity? result = await mongoPoc.GetByIdAsync(weatherCreated.Id);
    }
#endif

#if (UseLitedb)
    public async Task LitedbAsync()
    {
        LiteDbEntity entity = new() { Id = Guid.NewGuid().ToString(), Property = "Creado" };
        LiteDbEntity weatherCreated = await litedbPoc.CreateAsync(entity);
        weatherCreated.Property = "Modificado";
        LiteDbEntity? result = await litedbPoc.GetByIdAsync(weatherCreated.Id);
    }
#endif
}

internal static partial class Log
{
    [LoggerMessage(LogLevel.Information, "Executing example")]
    internal static partial void ExecutingTemplateLog(ILogger logger);
}
