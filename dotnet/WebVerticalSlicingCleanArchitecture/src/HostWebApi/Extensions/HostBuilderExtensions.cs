using Serilog;
using Serilog.Events;

namespace HostWebApi.Extensions;

public static class HostBuilderExtensions
{
    internal static void AddLoggerConfiguration(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Information()
                .Enrich.WithProperty("Application", "HostWebApi")
                .WriteTo.Console(LogEventLevel.Warning);

            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.WriteTo.Console(LogEventLevel.Debug);
            }
        });
    }

    internal static void AddMetricsAndTraces(this IServiceCollection services, IConfiguration configuration)
    {
    }
}
