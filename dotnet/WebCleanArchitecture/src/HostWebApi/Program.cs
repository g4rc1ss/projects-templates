using HostWebApi.Configurations;
using HostWebApi.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Diagnostics;
using Template.API;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.AddLoggerConfiguration(builder.Configuration);
builder.Services.AddMetricsAndTraces(builder.Configuration);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddProblemDetails();
builder.Services.AddOptions();
builder.Services.ConfigureDataProtectionProvider(builder.Configuration);


builder.Services.InitTemplate(builder.Configuration);


builder.Services.AddControllers(options =>
        options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseTransformer())))
    .AddApplicationPart(typeof(TemplateApiExtensions).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

Debug.WriteLine(app.Configuration["AppName"]!);

await app.RunAsync();

namespace HostWebApi
{
    public partial class Program
    {
    }
}