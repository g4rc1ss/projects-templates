using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace CompletedWeb.HostWebApi.Tests;

public sealed class ApiServiceFixture : IAsyncLifetime, IDisposable, IAsyncDisposable
{
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);

    public DistributedApplication? Application { get; private set; }
    public IDistributedApplicationTestingBuilder? Builder { get; private set; }

    public async Task InitializeAsync()
    {
        Builder?.Dispose();

        Builder =
            await DistributedApplicationTestingBuilder.CreateAsync<Projects.CompletedWeb_AspireHost>();
        Builder.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(Builder.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
            // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        });
        Builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        DistributedApplication app = await Builder.BuildAsync().WaitAsync(_defaultTimeout);

        if (Application is not null)
        {
            await Application.DisposeAsync();
        }

        Application = app;

        await app.StartAsync().WaitAsync(_defaultTimeout);
        await app
            .ResourceNotifications.WaitForResourceHealthyAsync("Host")
            .WaitAsync(_defaultTimeout);
    }

    public async Task DisposeAsync()
    {
        if (Application != null && Builder != null)
        {
            await Application.StopAsync();
            await Application.DisposeAsync();
            await Builder.DisposeAsync();
        }
    }

    public void Dispose()
    {
        Application?.Dispose();
        Builder?.Dispose();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (Application != null && Builder != null)
        {
            await Application.DisposeAsync();
            await Builder.DisposeAsync();
        }
    }
}
