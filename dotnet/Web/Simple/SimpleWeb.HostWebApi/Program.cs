using SimpleWeb.HostWebApi.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using SimpleWeb.HostWebApi.Configurations;
using SimpleWeb.HostWebApi.OpenAPI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 5 * 1024 * 1024);

builder.Configuration.AddUserSecrets<Program>().AddEnvironmentVariables();

// Add services to the container.
builder.InitSimpleWebHostConfig();

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer()));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.InitAndConfigureSwagger();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
if (!app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGrpcReflectionService();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");
app.MapControllers();

app.MapGrpcHealthChecksService();
app.MapRouteServices();

await app.RunAsync();
