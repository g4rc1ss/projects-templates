namespace WorkerTemplate.Workers.AzServiceBus;

public static class AzServiceBusExtensions
{
    internal static void AddAzureServiceBus(this IHostApplicationBuilder builder)
    {
        builder.AddAzureServiceBusClient("AzureServiceBus");
        builder.Services.AddHostedService<AzServiceBusWorker>();
    }
}