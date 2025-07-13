using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace SimpleWeb.HostWebApi.OpenAPI;

public static class ConfigureDocApi
{
    internal static void AddDocApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer(
                (document, _, _) =>
                {
                    Dictionary<string, OpenApiSecurityScheme> requirements = [];
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes = requirements;

                    return Task.CompletedTask;
                }
            );
        });
    }

    internal static void UseDocApi(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(
            "api-doc",
            options =>
            {
                options.AddPreferredSecuritySchemes();
            }
        );
    }
}
