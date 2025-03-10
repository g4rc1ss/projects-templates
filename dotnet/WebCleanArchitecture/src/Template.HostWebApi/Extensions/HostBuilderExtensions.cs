using Serilog;
using Serilog.Events;

namespace Template.HostWebApi.Extensions;

public static class HostBuilderExtensions
{
    internal static void AddLoggerConfiguration(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Warning()
                .Enrich.WithProperty("Application", "Template")
                .WriteTo.Console(LogEventLevel.Warning);

            if (context.HostingEnvironment.IsDevelopment())
            {
                // Verbose by default
                loggerConfiguration.WriteTo.Console();
            }
        });
    }

    internal static void AddMetricsAndTraces(this IServiceCollection services, IConfiguration configuration)
    {
    }
}