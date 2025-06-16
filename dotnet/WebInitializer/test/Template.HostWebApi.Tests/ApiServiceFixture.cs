using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace Template.HostWebApi.Tests;

public sealed class ApiServiceFixture : IAsyncLifetime
{
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);

    public required DistributedApplication Application { get; set; }

    public async Task InitializeAsync()
    {
        IDistributedApplicationTestingBuilder appHost =
            await DistributedApplicationTestingBuilder.CreateAsync<Projects.Template_AspireHost>();
        appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            // Override the logging filters from the app's configuration
            logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
            // To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
        });
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        DistributedApplication app = await appHost.BuildAsync().WaitAsync(_defaultTimeout);
        Application = app;

        await app.StartAsync().WaitAsync(_defaultTimeout);
        await app
            .ResourceNotifications.WaitForResourceHealthyAsync("Template")
            .WaitAsync(_defaultTimeout);
    }

    public async Task DisposeAsync()
    {
        await Application.StopAsync();
        await Application.DisposeAsync();
    }
}
