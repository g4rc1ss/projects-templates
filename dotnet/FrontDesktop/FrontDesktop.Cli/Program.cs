using System.CommandLine;
using FrontDesktop.Cli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// --file texto_prueba.txt

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

ParseResult parse = builder.CompileCommands(args);

IHost app = builder.Build();

IEnumerable<IHostedService> hostedServices = app.Services.GetRequiredService<IEnumerable<IHostedService>>();
foreach (IHostedService hostedService in hostedServices)
{
    await hostedService.StartAsync(CancellationToken.None);
}

FileInfo getFile = parse.GetRequiredValue(app.Services.GetRequiredService<Option<FileInfo>>());
Console.WriteLine(await File.ReadAllTextAsync(getFile.FullName));