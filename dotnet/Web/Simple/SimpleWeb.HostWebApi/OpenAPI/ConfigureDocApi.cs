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
                    Dictionary<string, OpenApiSecurityScheme> requirements = new()
                    {
#if (UseApiKey)
                        ["ApiKey"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.ApiKey,
                            Name = "X-API-KEY",
                            In = ParameterLocation.Header,
                            Description = "API Key",
                        },
#endif
#if (UseAzureIdentity)
                        ["Microsoft Login"] = new OpenApiSecurityScheme()
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Flows = new OpenApiOAuthFlows
                            {
                                AuthorizationCode = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri(
                                        $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize"
                                    ),
                                    TokenUrl = new Uri(
                                        $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token"
                                    ),
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { $"api://{clientId}/data.read", "Acceso a datos" },
                                    },
                                    Extensions = new Dictionary<string, IOpenApiExtension>
                                    {
                                        { "x-usePkce", new OpenApiString("SHA-256") },
                                    },
                                },
                            },
                        }
#endif
                    };
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
