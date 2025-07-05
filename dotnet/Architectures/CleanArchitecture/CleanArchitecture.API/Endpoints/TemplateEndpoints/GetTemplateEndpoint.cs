using CleanArchitecture.Application.UsesCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.API.Endpoints.TemplateEndpoints;

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
            // .RequireAuthorization()
            .WithName("GetTemplate")
            .WithDescription("Minimal api Template");

        return endpoints;
    }
}
