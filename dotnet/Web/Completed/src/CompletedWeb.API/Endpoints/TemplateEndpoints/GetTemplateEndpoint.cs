using CompletedWeb.Application.UsesCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace CompletedWeb.API.Endpoints.TemplateEndpoints;

public static class GetTemplateEndpoint
{
    public static IEndpointRouteBuilder MapGetTemplate(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "",
                async (
                    [FromServices] ILoggerFactory loggerFactory,
                    [FromServices] IExample example,
                    CancellationToken cancellationToken
                ) =>
                {
                    ILogger logger = loggerFactory.CreateLogger(nameof(GetTemplateEndpoint));
                    logger.LogInformation("Handler called");

                    await example.ExecuteAsync(cancellationToken);

                    return Results.Ok();
                }
            )
            .WithName("GetTemplate")
            .WithDisplayName("Get Template");

        return endpoints;
    }
}
