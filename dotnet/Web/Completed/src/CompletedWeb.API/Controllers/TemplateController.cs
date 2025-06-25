using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
using Microsoft.AspNetCore.Http;
#endif

namespace CompletedWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController(
    ILogger<TemplateController> logger
#if (!EventBusNone)
    ,
    IEventNotificator eventNotificator
#endif
#if (UseAzureCosmos)
    ,
    ICosmosdbPoc cosmosdb
#endif
#if (SqlDatabase && !UseIdentity)
    ,
    IUserRepository userRepository
#endif
#if (UseIdentity)
    ,
    IIdentityUserRepository userRepository
#endif
#if (UseMongodb)
    ,
    IMongoPoc mongoPoc
#endif
#if (!StorageNone)
    ,
    IFileStorage storage
#endif
) : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        logger.LogInformation("Get called");
        if (HttpContext.ToString() is null) { }

        return Ok();
    }

#if (!EventBusNone)
    [HttpGet("Events")]
    public async Task<IActionResult> EventsAsync()
    {
        await eventNotificator.PublishAsync(new RequestMessage());
        return Ok();
    }
#endif

#if (UseAzureCosmos)
    [HttpGet("Cosmos")]
    public async Task<IActionResult> CosmosAsync()
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

        return Ok(result);
    }
#endif

#if (SqlDatabase && !UseIdentity)
    [HttpGet("SqlDatabase")]
    public async Task<IActionResult> SqlDatabaseAsync()
    {
        UserEntity user = new() { Name = "Nombre" };
        UserEntity userCreated = await userRepository.CreateAsync(user);
        userCreated.Name = "Nombre Modificado";

        UserEntity? result = await userRepository.GetByIdAsync(userCreated.Id);
        return Ok(result);
    }
#endif

#if (UseIdentity)
    [HttpGet("Identity")]
    public async Task<IActionResult> IdentityAsync()
    {
        IdentityUserEntity user = new() { UserName = "Nombre" };
        IdentityUserEntity userCreated = await userRepository.CreateAsync(user);
        userCreated.UserName = "Nombre Modificado";

        IdentityUserEntity? result = await userRepository.GetByIdAsync(userCreated.Id.ToString());
        return Ok(result);
    }
#endif

#if (UseMongodb)
    [HttpGet("mongodb")]
    public async Task<IActionResult> MongodbAsync()
    {
        MongoDbEntity entity = new() { Id = Guid.NewGuid().ToString(), Property = "Creado" };
        MongoDbEntity userCreated = await mongoPoc.CreateAsync(entity);
        userCreated.Property = "Modificado";
        MongoDbEntity? result = await mongoPoc.GetByIdAsync(userCreated.Id);
        return Ok(result);
    }
#endif

#if (!StorageNone)
    [HttpGet("Storage")]
    public async Task<IActionResult> StorageAsync(IFormFile file)
    {
        string idDocumento = Guid.NewGuid().ToString();
        string path = $"{idDocumento}/{file.FileName}";

        await storage.UploadFileAsync(file.OpenReadStream(), "blob", path);
        Stream result = await storage.DownloadFileAsync("blob", path);
        return Ok();
    }
#endif
}
