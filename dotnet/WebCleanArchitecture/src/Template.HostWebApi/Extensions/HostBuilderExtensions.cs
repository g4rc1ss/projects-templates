using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace Template.HostWebApi.Extensions;

public static class HostBuilderExtensions
{
    internal static void ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource =>
                resource.AddService(builder.Configuration["AppName"]!))
            .WithMetrics(metric =>
            {
                metric.AddMeter(builder.Configuration["AppName"]!);
                metric.AddAspNetCoreInstrumentation();
                metric.AddRuntimeInstrumentation();
                metric.AddHttpClientInstrumentation();
                metric.AddProcessInstrumentation();
            })
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
            });

        builder.AddOtelExporter();
    }

    private static void AddOtelExporter<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        bool useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("OpenTelemetry")))
        {
            builder.Services.AddOpenTelemetry()
                .UseOtlpExporter(OtlpExportProtocol.Grpc,
                    new Uri(builder.Configuration.GetConnectionString("OpenTelemetry")!));
        }

        // To Enable on Azure Monitor(requires Azure.Monitor.OpenTelemetry.AspNetCore package)
        // string? insightsConnection = builder.Configuration
        //     .GetConnectionString("Insights");
        // builder.Services.AddOpenTelemetry()
        //     .UseAzureMonitor();
    }
}