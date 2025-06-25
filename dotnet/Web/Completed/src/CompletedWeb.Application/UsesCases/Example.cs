using CompletedWeb.Application.Contracts;
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
#if (UseAzureCosmos)
    ICosmosdbPoc cosmosdb,
#endif
#if (SqlDatabase && !UseIdentity)
    IUserRepository userRepository,
#endif
#if (UseIdentity)
    IIdentityUserRepository userRepository,
#endif
#if (UseMongodb)
    IMongoPoc mongoPoc,
#endif
#if (!StorageNone)
    IFileStorage storage
#endif
) : IExample
{
    public async Task<Result> ExecuteAsync(CancellationToken cancellationToken = default)
    {
#if (!EventBusNone)
        await EventsAsync();
#endif
#if (UseAzureCosmos)
        await CosmosAsync();
#endif
#if (SqlDatabase && !UseIdentity)
        await SqlDatabaseAsync();
#endif
#if (UseIdentity)
        await IdentityAsync();
#endif
#if (UseMongodb)
        await MongodbAsync();
#endif
#if (!StorageNone)
        await StorageAsync();
#endif
        return Result.Success();
    }

#if (!EventBusNone)
    private async Task EventsAsync()
    {
        await eventNotificator.PublishAsync(new RequestMessage());
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

#if (SqlDatabase && !UseIdentity)
    private async Task SqlDatabaseAsync()
    {
        UserEntity user = new() { Name = "Nombre" };
        UserEntity userCreated = await userRepository.CreateAsync(user);
        userCreated.Name = "Nombre Modificado";

        UserEntity? result = await userRepository.GetByIdAsync(userCreated.Id);
    }
#endif

#if (UseIdentity)
    private async Task IdentityAsync()
    {
        IdentityUserEntity user = new() { UserName = "Nombre" };
        IdentityUserEntity userCreated = await userRepository.CreateAsync(user);
        userCreated.UserName = "Nombre Modificado";

        IdentityUserEntity? result = await userRepository.GetByIdAsync(userCreated.Id.ToString());
    }
#endif

#if (UseMongodb)
    private async Task MongodbAsync()
    {
        MongoDbEntity entity = new() { Id = Guid.NewGuid().ToString(), Property = "Creado" };
        MongoDbEntity userCreated = await mongoPoc.CreateAsync(entity);
        userCreated.Property = "Modificado";
        MongoDbEntity? result = await mongoPoc.GetByIdAsync(userCreated.Id);
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