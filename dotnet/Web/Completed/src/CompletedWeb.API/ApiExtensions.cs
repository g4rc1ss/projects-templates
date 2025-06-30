using CompletedWeb.API.Endpoints.TemplateEndpoints;
using CompletedWeb.API.Endpoints.ValidateEnpoints;
using CompletedWeb.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
        endpoints.MapGroup("validate").WithTags("Validate").MapPostPocValidate();

        endpoints.MapGroup("template").WithTags("Template").MapGetTemplate();

        return endpoints;
    }
}
