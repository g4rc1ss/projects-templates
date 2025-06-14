﻿using Template.HostWebApi.Extensions;
#if (UseApi)
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Template.HostWebApi.Configurations;
using Template.HostWebApi.FilterControllers;
using Template.HostWebApi.OpenAPI;
#endif

#if (SqlDatabase)
using Template.HostWebApi.HostedServices;
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
#endif

#if (SqlDatabase)
builder.Services.AddHostedService<MigrationHostedService>();
#endif

#if (UseAzureOpts)
builder.AddAzureServices();
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
app.UseAuthentication();
app.UseAuthorization();
#endif

#if (UseApi)
app.MapHealthChecks("/health");
app.MapControllers();
#endif

#if (UseGrpc)
app.MapGrpcHealthChecksService();
#endif
app.MapRouteServices();

await app.RunAsync();

namespace Template.HostWebApi
{
    public class Program;
}
