#if (UseAzureCosmos)
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        CosmosDbEntity cosmosEntity = new() { Id = Guid.NewGuid().ToString(), Property = "Propiedad" };

        CosmosDbEntity entidadCreada = await cosmosdb.CreateAsync(cosmosEntity);
        entidadCreada.Property = "Propiedad Modificada";
        await cosmosdb.UpdateAsync(entidadCreada);

        CosmosDbEntity? result = await cosmosdb.GetByIdAsync(entidadCreada.Id);

        return Ok(result);
    }
#endif

#if (!StorageNone)
    [HttpGet("Storage")]
    public async Task<IActionResult> StorageAsync()
    {
        byte[] contenido = "Este es un contenido de prueba a guardar"u8.ToArray();
        await storage.UploadFileAsync(contenido, "/blob", "nombreBlob");
        Stream result = await storage.DownloadFileAsync("/blob", "nombreBlob");
        return Ok();
    }
#endif
}