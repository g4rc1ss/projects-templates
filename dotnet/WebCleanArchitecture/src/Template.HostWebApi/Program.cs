#if (UseApi)
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Template.HostWebApi.Configurations;
using Template.HostWebApi.FilterControllers;
using Template.HostWebApi.OpenAPI;
#endif
using Template.HostWebApi.Extensions;
#if (SqlDatabase)
using Template.HostWebApi.Services;
#endif

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 5 * 1024 * 1024);

builder.Configuration.AddUserSecrets<Program>()
    .AddEnvironmentVariables();

// Add services to the container.
builder.InitTemplateHostConfig();

#if (UseGrp)
builder.Services.AddGrpc();
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

WebApplication app = builder.Build();

#if (UseApi)
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
#endif

app.MapHealthChecks("/health");
#if (UseApi)
app.MapControllers();
#endif

await app.RunAsync();

namespace Template.HostWebApi
{
    public class Program;
}