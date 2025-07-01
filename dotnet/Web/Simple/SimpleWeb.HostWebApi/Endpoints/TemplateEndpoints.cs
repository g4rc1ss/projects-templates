using Microsoft.AspNetCore.Mvc;
using SimpleWeb.HostWebApi.UsesCases;

namespace SimpleWeb.HostWebApi.Endpoints;

internal static class TemplateEndpoints
{
    internal static IEndpointRouteBuilder MapGetTemplate(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("", HandlerAsync);

        return endpoints;
    }

    private static async Task<IResult> HandlerAsync(
        HttpContext httpContext,
        [FromServices] Example example,
        [FromServices] ILoggerFactory loggerFactory
    )
    {
        ILogger logger = loggerFactory.CreateLogger(typeof(TemplateEndpoints));

        logger.LogInformation("Get called");
        if (httpContext.ToString() is null) { }

        await example.ExecuteAsync();

        return Results.Ok();
    }
}
