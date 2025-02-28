﻿using System.Diagnostics;
using System.Threading.RateLimiting;
using HostWebApi.Extensions;
using Template.API;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.AddLoggerConfiguration(builder.Configuration);
builder.Services.AddMetricsAndTraces(builder.Configuration);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.InicializarConfiguracionApp(builder.Configuration);
builder.Services.AddProblemDetails();


builder.Services.AddControllers()
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

public partial class Program
{
}
