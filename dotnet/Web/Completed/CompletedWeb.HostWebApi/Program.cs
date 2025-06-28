using CompletedWeb.HostWebApi.Extensions;
#if (UseGrpc)
#if (UseLayerArchitecture)
using CompletedWeb.Grpc;
#endif
#endif
#if (UseApi)
using CompletedWeb.HostWebApi.OpenAPI;
#if (!AuthNone)
using Infraestructure.Auth;
#endif
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 5 * 1024 * 1024);

builder.Configuration.AddUserSecrets<Program>().AddEnvironmentVariables();

// Add services to the container.
builder.InitHostWebConfig();

#if (UseApi)
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("RequestId", context.HttpContext.TraceIdentifier);
    };
});
#endif

#if (UseGrpc)
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
#endif

#if (UseApi)
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.InitAndConfigureSwagger();
builder.Services.AddSingleton<SwaggerAuthMiddleware>();
#endif

#if (!KeyVaultNone)
builder.AddKeyVault();
#endif

WebApplication app = builder.Build();

#if (UseApi)
// Configure the HTTP request pipeline.
app.UseExceptionHandler();
#endif
if (!app.Environment.IsProduction())
{
#if (UseApi)
    app.UseDeveloperExceptionPage();
    app.UseMiddleware<SwaggerAuthMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();
#endif
#if (UseGrpc)
    app.MapGrpcReflectionService();
#endif
}

#if (UseApi)
app.UseHttpsRedirection();
#endif

#if (!AuthNone)
app.UseAuthentication();
app.UseAuthorization();

#endif
#if (UseGrpc)
app.MapGrpcHealthChecksService();
#endif
#if (UseApi)
app.MapHealthChecks("/health");
#if (!AuthNone)

app.MapAuthEndpoints();
#endif
#endif

app.MapHostEndpointsServices();

await app.RunAsync();

namespace CompletedWeb.HostWebApi
{
    public class Program;
}
