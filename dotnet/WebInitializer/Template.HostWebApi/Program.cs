using Template.HostWebApi.Extensions;
#if (UseGrpc)
#if (UseLayerArchitecture)
using Template.Grpc;
#endif
#endif

#if (!StorageNone)
using Infraestructure.Storages;
#endif

#if (UseApi)
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Template.HostWebApi.Configurations;
using Template.HostWebApi.FilterControllers;
using Template.HostWebApi.OpenAPI;
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 5 * 1024 * 1024);

builder.Configuration.AddUserSecrets<Program>().AddEnvironmentVariables();

// Add services to the container.
builder.InitTemplateHostConfig();

#if (UseGrpc)
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
#endif

#if (UseApi)
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer()));
    options.Filters.Add<ResultResponseFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.InitAndConfigureSwagger();
builder.Services.AddSingleton<SwaggerAuthMiddleware>();
#endif

#if (!KeyVaultNone)
builder.AddKeyVault();
#endif
#if (!StorageNone)
builder.AddStorages();
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
#if (UseApi)
app.MapHealthChecks("/health");
app.MapControllers();
#endif

#if (UseGrpc)
#if (UseLayerArchitecture)
app.MapTemplateGrpc();
#endif
app.MapGrpcHealthChecksService();
#endif
app.MapRouteServices();

await app.RunAsync();

namespace Template.HostWebApi
{
    public class Program;
}
