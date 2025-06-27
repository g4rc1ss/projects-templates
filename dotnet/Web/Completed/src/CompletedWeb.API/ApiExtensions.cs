using CompletedWeb.API.Endpoints.TemplateEndpoints;
using CompletedWeb.API.Endpoints.ValidateEnpoints;
using CompletedWeb.Application;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace CompletedWeb.API;

public static class ApiExtensions
{
    public static void InitWebApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddBusinessServices();
    }

    public static IEndpointRouteBuilder MapWebApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapValidateEndpoints();
        endpoints.MapTemplateEndpoints();

        return endpoints;
    }
}
