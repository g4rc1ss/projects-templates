#if (SqlDatabase)
using Infraestructure.Database;
using Microsoft.Extensions.Hosting;
#endif

namespace FrontDesktop.iOS;

public class Application
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }

#if (SqlDatabase)
    public static IHostApplicationBuilder CreateHostBuilder(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.AddDatabaseConfig();
        return builder;
    }
#endif
}