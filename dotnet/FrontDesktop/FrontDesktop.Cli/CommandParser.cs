using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FrontDesktop.Cli;

public static class CommandParser
{
    public static ParseResult CompileCommands(this IHostApplicationBuilder builder, string[] args)
    {
        RootCommand? rootCommand = new(
            "Ejemplo de la nueva api de Microsoft para Argumentos de comandos para CLI"
        );

        Option<FileInfo>? fileOpt = new(name: "--file") { Description = "The file to read" };
        rootCommand.Options.Add(fileOpt);

        builder.Services.AddSingleton(fileOpt);

        return rootCommand.Parse(args);
    }
}
