using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace SimpleWeb.HostWebApi.Extensions;

public static class TelemetryExtensions
{
    internal static void ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        if (!builder.Environment.IsProduction())
        {
            builder.AddDeveloperOpenTelemetry();
        }

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeScopes = true;
            logging.IncludeFormattedMessage = true;
        });

        builder
            .Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
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
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        // Guardamos las consultas generadas por EF
                        options.SetDbStatementForText = true;
                    })
            );

        builder.AddOtelExporter();
    }

    private static void AddDeveloperOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry().WithTracing(trace => { }).WithMetrics(metric => { });
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
