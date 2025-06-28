using CompletedWeb.API.Endpoints.TemplateEndpoints.GetTemplate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace CompletedWeb.API.Endpoints.TemplateEndpoints;

public static class TemplateGroupEndpoints
{
    public static void MapTemplateEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("Template").MapGetTemplateEndpoint();
    }
}
