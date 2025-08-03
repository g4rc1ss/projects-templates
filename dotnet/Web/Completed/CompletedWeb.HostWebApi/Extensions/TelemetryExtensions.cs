using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace CompletedWeb.HostWebApi.Extensions;

public static class TelemetryExtensions
{
    internal static void ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeScopes = true;
            logging.IncludeFormattedMessage = true;
        });
        builder.Logging.SetMinimumLevel(LogLevel.Warning);

        builder
            .Services.AddOpenTelemetry()
            .WithMetrics(metric =>
                metric
                    .AddMeter(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddHttpClientInstrumentation()
            )
            .WithTracing(trace =>
                trace
                    .AddAspNetCoreInstrumentation()
#if (UseGrpc)
                    .AddGrpcClientInstrumentation()
#endif
                    .AddHttpClientInstrumentation()
            );

        if (!builder.Environment.IsProduction())
        {
            builder.AddDeveloperOpenTelemetry();
        }

        builder.AddOtelExporter();
    }

    private static void AddDeveloperOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry().WithTracing(trace => { }).WithMetrics(metric => { });
        builder.Logging.SetMinimumLevel(LogLevel.Information);
    }

    private static void AddOtelExporter<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        // Variables para la configuracion:
        // OTEL_EXPORTER_OTLP_ENDPOINT: http://...
        // OTEL_EXPORTER_OTLP_PROTOCOL: grpc o http/protobuf
        // OTEL_EXPORTER_OTLP_HEADERS: key=value, (Si hay mas de 1 se separan por comas)

        bool useOtlpExporter = !string.IsNullOrWhiteSpace(
            builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]
        );

        if (useOtlpExporter)
        {
            OpenTelemetryBuilder otel = builder.Services.AddOpenTelemetry();
            otel.UseOtlpExporter();
        }

        // To Enable on Azure Monitor(requires Azure.Monitor.OpenTelemetry.AspNetCore package)
        // if (!string.IsNullOrWhiteSpace(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        // {
        //     builder.Services.AddOpenTelemetry()
        //         .UseAzureMonitor();
        // }
    }
}
