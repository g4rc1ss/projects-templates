﻿using SimpleWeb.HostWebApi.Extensions;
using SimpleWeb.HostWebApi.OpenAPI;
#if (UseMvc)
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SimpleWeb.HostWebApi.Configurations;
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 5 * 1024 * 1024);

builder.Configuration.AddUserSecrets<Program>().AddEnvironmentVariables();

// Add services to the container.
builder.InitWebHostConfig();

builder.AddDocApi();

#if (UseGrpc)
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
#endif

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("RequestId", context.HttpContext.TraceIdentifier);
    };
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseDocApi();
#if (UseGrpc)
    app.MapGrpcReflectionService();
#endif
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

#if (UseGrpc)
app.MapGrpcHealthChecksService();
#endif
app.MapRouteServices();

await app.RunAsync();
