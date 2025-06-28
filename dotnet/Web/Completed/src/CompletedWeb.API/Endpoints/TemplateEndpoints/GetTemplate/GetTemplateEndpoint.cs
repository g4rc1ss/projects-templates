using CompletedWeb.Application.UsesCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace CompletedWeb.API.Endpoints.TemplateEndpoints.GetTemplate;

public static class GetTemplateEndpoint
{
    public static IEndpointRouteBuilder MapGetTemplateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("", HandlerAsync).WithName("GetTemplate").WithDisplayName("Get Template");

        return endpoints;
    }

    private static async Task<IResult> HandlerAsync(
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] IExample example,
        CancellationToken cancellationToken
    )
    {
        ILogger logger = loggerFactory.CreateLogger(nameof(GetTemplateEndpoint));
        logger.LogInformation("Handler called");

        await example.ExecuteAsync(cancellationToken);

        return Results.Ok();
    }
}
