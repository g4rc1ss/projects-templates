using System.CommandLine;
using FrontDesktop.Cli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

// --file texto_prueba.txt

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Services.AddSerilog(
    (configuration) =>
    {
        configuration.WriteTo.File("./logs.txt", LogEventLevel.Information);
    }
);

ParseResult parse = builder.CompileCommands(args);

IHost app = builder.Build();

IEnumerable<IHostedService> hostedServices = app.Services.GetRequiredService<
    IEnumerable<IHostedService>
>();

foreach (IHostedService hostedService in hostedServices)
{
    await hostedService.StartAsync(CancellationToken.None);
}

ILoggerFactory loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Starting application");

FileInfo getFile = parse.GetRequiredValue(app.Services.GetRequiredService<Option<FileInfo>>());
Console.WriteLine(await File.ReadAllTextAsync(getFile.FullName));
