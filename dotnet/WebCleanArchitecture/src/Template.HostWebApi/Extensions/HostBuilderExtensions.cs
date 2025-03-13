using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace Template.HostWebApi.Extensions;

public static class HostBuilderExtensions
{
    internal static void AddLoggerConfiguration(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Warning()
                .Enrich.WithProperty("Application", configuration["AppName"]!)
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = configuration.GetConnectionString("OpenTelemetry");
                });

            if (!context.HostingEnvironment.IsProduction())
            {
                loggerConfiguration
                    .MinimumLevel.Debug()
                    .WriteTo.Console();
            }
        });
    }

    internal static void AddMetricsAndTraces(this WebApplicationBuilder builder)
    {
        string? otelConnectionString = builder.Configuration
            .GetConnectionString("OpenTelemetry");
        if (string.IsNullOrWhiteSpace(otelConnectionString)
            && builder.Environment.EnvironmentName.ToLower() == "local")
        {
            return;
        }
        
        ArgumentNullException.ThrowIfNull(otelConnectionString);
        
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
                resource.AddService(builder.Configuration["AppName"]!))
            .WithTracing(trace =>
            {
                trace.AddAspNetCoreInstrumentation();
                trace.AddHttpClientInstrumentation();
                // trace.AddSource(nameof(IDistributedCache));
                // trace.AddEntityFrameworkCoreInstrumentation(options =>
                // {
                //     // Guardamos las consultas generadas por EF
                //     options.SetDbStatementForText = true;
                // });
                trace.AddOtlpExporter(
                    exporter => exporter.Endpoint = new Uri(otelConnectionString));
            })
            .WithMetrics(metric =>
            {
                metric.AddMeter(builder.Configuration["AppName"]!);
                metric.AddAspNetCoreInstrumentation();
                metric.AddRuntimeInstrumentation();
                metric.AddHttpClientInstrumentation();
                metric.AddProcessInstrumentation();

                metric.AddOtlpExporter(exporter =>
                {
                    exporter.Endpoint = new Uri(otelConnectionString);
                    exporter.Protocol = OtlpExportProtocol.Grpc;
                });
            });
    }
}