using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using MvcArchitecture.API.Endpoints.TemplateEndpoints;

namespace MvcArchitecture.API;

public static class ApiExtensions
{
    public static void InitApi(this IHostApplicationBuilder builder)
    {
    }

    public static IEndpointRouteBuilder MapApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("template").WithTags("Template").MapGetTemplate();

        return endpoints;
    }
}
