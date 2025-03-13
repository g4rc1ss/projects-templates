using Template.HostWebApi.Configurations;
using Template.HostWebApi.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Shared;
using System.Diagnostics;
using System.Security.Claims;
using Functionality.API;
using Template.HostWebApi.FilterControllers;
using Template.HostWebApi.OpenAPI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 5 * 1024 * 1024);

builder.Configuration.AddUserSecrets<Program>();

builder.Host.AddLoggerConfiguration(builder.Configuration);
builder.AddMetricsAndTraces();

// Add services to the container.
builder.InitTemplateHostConfig();

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer()));
    options.Filters.Add<ResultResponseFilter>();
}).AddApplicationPart(typeof(FunctionalityApiExtensions).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.InitAndConfigureSwagger();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseMiddleware<SwaggerAuthMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Debug.WriteLine(app.Configuration["AppName"]!);

await app.RunAsync();

namespace Template.HostWebApi
{
    public class Program;
}