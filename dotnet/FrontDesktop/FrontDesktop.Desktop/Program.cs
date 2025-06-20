using Avalonia;
#if (SqlDatabase)
using Infraestructure.Database;
using Microsoft.Extensions.Hosting;
#endif

namespace FrontDesktop.Desktop;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) =>
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace();

#if (SqlDatabase)
    public static IHostApplicationBuilder CreateHostBuilder(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.AddDatabaseConfig();
        return builder;
    }
#endif
}
