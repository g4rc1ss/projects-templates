using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Template.HostWebApi.Configurations;
using Template.HostWebApi.Extensions;
using Template.HostWebApi.FilterControllers;
using Template.HostWebApi.OpenAPI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 5 * 1024 * 1024);

builder.Configuration.AddUserSecrets<Program>()
    .AddEnvironmentVariables();

// Add services to the container.
builder.InitTemplateHostConfig();

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer()));
    options.Filters.Add<ResultResponseFilter>();
});

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

app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();

namespace Template.HostWebApi
{
    public class Program;
}