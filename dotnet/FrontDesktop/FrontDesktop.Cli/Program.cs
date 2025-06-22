using System.CommandLine;
using FrontDesktop.Cli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if (SqlDatabase)
using FrontDesktop.Cli.HostedServices;
#endif

#if (!DatabaseNone)
using Infraestructure.Database;
#endif
#if (!EventBusNone)
using Infraestructure.Events;
#endif
#if (!StorageNone)
using Infraestructure.Storages;
#endif

// --file texto_prueba.txt

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

ParseResult parse = builder.CompileCommands(args);

#if (SqlDatabase)
        builder.Services.AddSingleton<MigrationsServices>();
        builder.Services.AddHostedService<MigrationsServices>();
#endif

#if (!DatabaseNone)
builder.AddDatabaseConfig();
#endif
#if (!EventBusNone)
builder.AddEventsServices();
#endif
#if (!StorageNone)
builder.AddStorages();
#endif

IHost app = builder.Build();

IEnumerable<IHostedService> hostedServices = app.Services.GetRequiredService<IEnumerable<IHostedService>>();
foreach (IHostedService hostedService in hostedServices)
{
    await hostedService.StartAsync(CancellationToken.None);
}

FileInfo getFile = parse.GetRequiredValue(app.Services.GetRequiredService<Option<FileInfo>>());
Console.WriteLine(await File.ReadAllTextAsync(getFile.FullName));