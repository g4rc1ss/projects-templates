using CompletedWeb.Application.Contracts;
using Microsoft.Extensions.Logging;
#if (UseAzServiceBus)
using Microsoft.Extensions.Configuration;
#endif
#if (SqlDatabase || NoSqlDatabase)
using Infraestructure.Database.Entities;
using Infraestructure.Database.Repository;
#endif
#if (!EventBusNone)
using Infraestructure.Events.Messages.Models;
using Infraestructure.Events.Publisher;
#endif
#if (!StorageNone)
using Infraestructure.Storages;
#endif

namespace CompletedWeb.Application.UsesCases;

public class Example(
#if (!EventBusNone)
    IEventNotificator eventNotificator,
#endif
#if (UseAzServiceBus)
    IConfiguration configuration,
#endif
#if (UseAzureCosmos)
    ICosmosdbPoc cosmosdb,
#endif
#if (UsePostgres)
    IPostgresPoc postgresPoc,
#endif
#if (UseSqlite)
    ISqlitePoc sqlitePoc,
#endif
#if (UseSqlServer)
    ISqlServerPoc sqlServerPoc,
#endif
#if (UseMongodb)
    IMongoPoc mongoPoc,
#endif
#if (!StorageNone)
    IFileStorage storage,
#endif
    ILogger<Example> logger
) : IExample
{
    public async Task<Result> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Executing example");
#if (!EventBusNone)
        await EventsAsync();
#endif
#if (UseAzureCosmos)
        await CosmosAsync();
#endif
#if (UsePostgres)
        await PostgresAsync();
#endif
#if (UseSqlite)
        await SqliteAsync();
#endif
#if (UseSqlServer)
        await SqlServerAsync();
#endif
#if (UseMongodb)
        await MongodbAsync();
#endif
#if (!StorageNone)
        await StorageAsync();
#endif
        return new Result();
    }

#if (!EventBusNone)
    private async Task EventsAsync()
    {
        Dictionary<string, string>? additionalProperties = [];
#if (UseAzServiceBus)
        string? queueName = configuration.GetSection("AzServiceBusConfig")["QueueName"];
        ArgumentNullException.ThrowIfNull(queueName);
        additionalProperties.Add("AzQueueName", queueName);

#endif
#if (UseRabbitMQ)
        additionalProperties.Add("routingKey", "Template.mensajePrueba");
        additionalProperties.Add("exchange", "subscription.exchange");
#endif
        RequestMessage message = new();
        await eventNotificator.PublishAsync(message, additionalProperties);
    }

#endif
#if (UseAzureCosmos)
    private async Task CosmosAsync()
    {
        CosmosDbEntity cosmosEntity = new()
        {
            Id = Guid.NewGuid().ToString(),
            Property = "Propiedad",
        };

        CosmosDbEntity entidadCreada = await cosmosdb.CreateAsync(cosmosEntity);
        entidadCreada.Property = "Propiedad Modificada";
        await cosmosdb.UpdateAsync(entidadCreada);

        CosmosDbEntity? result = await cosmosdb.GetByIdAsync(entidadCreada.Id);
    }

#endif
#if (UsePostgres)
    private async Task PostgresAsync()
    {
        WeatherForecastEntity weatherForecast = new() { Name = "Nombre" };
        WeatherForecastEntity weatherForecastCreated = await postgresPoc.CreateAsync(
            weatherForecast
        );
        weatherForecastCreated.Name = "Nombre Modificado";

        WeatherForecastEntity? result = await postgresPoc.GetByIdAsync(weatherForecastCreated.Id);
    }

#endif
#if (UseSqlite)
    private async Task SqliteAsync()
    {
        WeatherForecastEntity weatherForecast = new() { Name = "Nombre" };
        WeatherForecastEntity weatherForecastCreated = await sqlitePoc.CreateAsync(weatherForecast);
        weatherForecastCreated.Name = "Nombre Modificado";

        WeatherForecastEntity? result = await sqlitePoc.GetByIdAsync(weatherForecastCreated.Id);
    }

#endif
#if (UseSqlServer)
    private async Task SqlServerAsync()
    {
        WeatherForecastEntity weatherForecast = new() { Name = "Nombre" };
        WeatherForecastEntity weatherForecastCreated = await sqlServerPoc.CreateAsync(
            weatherForecast
        );
        weatherForecastCreated.Name = "Nombre Modificado";

        WeatherForecastEntity? result = await sqlServerPoc.GetByIdAsync(weatherForecastCreated.Id);
    }

#endif
#if (UseMongodb)
    private async Task MongodbAsync()
    {
        MongoDbEntity entity = new() { Id = Guid.NewGuid().ToString(), Property = "Creado" };
        MongoDbEntity weatherForecastCreated = await mongoPoc.CreateAsync(entity);
        weatherForecastCreated.Property = "Modificado";
        MongoDbEntity? result = await mongoPoc.GetByIdAsync(weatherForecastCreated.Id);
    }

#endif
#if (!StorageNone)
    private async Task StorageAsync()
    {
        string idDocumento = Guid.NewGuid().ToString();
        string path = $"{idDocumento}/nombre-archivo";

        await storage.UploadFileAsync("contenidoArchivo"u8.ToArray(), "blob", path);
        Stream result = await storage.DownloadFileAsync("blob", path);
    }
#endif
}

public interface IExample : IApplicationContract;
