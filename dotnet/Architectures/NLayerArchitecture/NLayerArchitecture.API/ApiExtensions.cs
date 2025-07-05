using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using NLayerArchitecture.API.Endpoints.TemplateEndpoints;
using NLayerArchitecture.Application;

namespace NLayerArchitecture.API;

public static class ApiExtensions
{
    public static void InitApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
    }

    public static IEndpointRouteBuilder MapApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("template").WithTags("Template").MapGetTemplate();

        return endpoints;
    }
}
