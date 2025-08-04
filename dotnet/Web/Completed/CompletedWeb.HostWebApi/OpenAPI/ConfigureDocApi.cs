using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
#if (UseApiKey)
using Infraestructure.Auth.ApiKey;
#endif

#if (UseIdentity || UseJwt)
using Microsoft.AspNetCore.Authentication.JwtBearer;
#endif
#if (UseAzureAD)
using Infraestructure.Auth.AzureAD;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
#endif

namespace CompletedWeb.HostWebApi.OpenAPI;

public static class ConfigureDocApi
{
    internal static void AddDocApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
                {
#if (UseAzureAD)
                    AzureAdOptions? azureAd = builder
                        .Configuration.GetSection("AzureAd")
                        .Get<AzureAdOptions>();
#endif

                    Dictionary<string, OpenApiSecurityScheme> requirements = new()
                    {
#if (UseIdentity || UseJwt)
                        ["Bearer"] = new OpenApiSecurityScheme
                        {
                            BearerFormat = "JWT",
                            Name = "JWT Authentication",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme,
                            },
                        },
#endif
#if (UseApiKey)
                        [Constants.API_KEY_SCHEME] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.ApiKey,
                            Name = Constants.API_KEY_HEADER_NAME,
                            In = ParameterLocation.Header,
                            Description = "API Key",
                        },
#endif
#if (UseAzureAD)
                        ["Microsoft Login AD"] = new OpenApiSecurityScheme()
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Flows = new OpenApiOAuthFlows
                            {
                                AuthorizationCode = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri(
                                        $"https://login.microsoftonline.com/{azureAd?.TenantId}/oauth2/v2.0/authorize"
                                    ),
                                    TokenUrl = new Uri(
                                        $"https://login.microsoftonline.com/{azureAd?.TenantId}/oauth2/v2.0/token"
                                    ),
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { $"api://{azureAd?.ClientId}/{azureAd?.Scope}", "Acceso a datos" },
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

                    document.SecurityRequirements =
                    [
                        new OpenApiSecurityRequirement
                        {
#if (UseIdentity || UseJwt)
                            [requirements["Bearer"]] = Array.Empty<string>(),
#endif
#if (UseAzureAD)
                            [requirements["Microsoft Login AD"]] = Array.Empty<string>(),
#endif
#if (UseApiKey)
                            [requirements["Constants.API_KEY_SCHEME"]] = Array.Empty<string>(),
#endif
                        },
                    ];

                    return Task.CompletedTask;
                }
            );
        });
    }

    internal static void UseDocApi(this WebApplication app)
    {
        app.MapOpenApi();


#if (UseAzureAD)
        AzureAdOptions? azureAd = app.Configuration.GetSection("AzureAd").Get<AzureAdOptions>();
#endif
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "CompletedWeb");
            // options.RoutePrefix = "api-doc";

#if (UseAzureAD)
            options.OAuthClientId(azureAd?.ClientId);
            options.OAuthScopes($"api://{azureAd?.ClientId}/{azureAd?.Scope}");
            options.OAuthUsePkce();
            options.EnablePersistAuthorization();
#endif
        });

        app.MapScalarApiReference(
            "api-doc",
            options =>
            {
                options.AddPreferredSecuritySchemes();
#if (UseAzureAD)
                options.AddAuthorizationCodeFlow(
                    "Microsoft Login AD",
                    flow =>
                    {
                        flow.ClientId = azureAd?.ClientId;
                    }
                );
#endif
            }
        );
    }
}