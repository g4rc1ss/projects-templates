using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace MvcArchitecture.API.Endpoints.TemplateEndpoints;

public static class GetTemplateEndpoint
{
    public static IEndpointRouteBuilder MapGetTemplate(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "",
                async (
                    [FromServices] ILoggerFactory loggerFactory,
                    CancellationToken cancellationToken
                ) =>
                {
                    ILogger logger = loggerFactory.CreateLogger(nameof(GetTemplateEndpoint));
                    logger.LogInformation("Handler called");

                    return Results.Ok();
                }
            )
            // .RequireAuthorization()
            .WithName("GetTemplate")
            .WithDescription("Minimal api Template");

        return endpoints;
    }
}
