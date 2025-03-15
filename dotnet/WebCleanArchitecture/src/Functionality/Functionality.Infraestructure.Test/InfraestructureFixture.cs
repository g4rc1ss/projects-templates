using Aspire.Hosting;

namespace Functionality.Infraestructure.Test;

public sealed class InfraestructureFixture : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    public required DistributedApplication Application { get; set; }

    public async Task InitializeAsync()
    {
        IDistributedApplicationTestingBuilder appHost =
            await DistributedApplicationTestingBuilder.CreateAsync<Projects.Template_AspireHost>(
            [
                "UseVolumes=false"
            ]);
        appHost.Environment.EnvironmentName = "Testing";

        DistributedApplication app = await appHost.BuildAsync().WaitAsync(DefaultTimeout);
        Application = app;

        await app.StartAsync().WaitAsync(DefaultTimeout);
        await app.ResourceNotifications.WaitForResourceHealthyAsync("Template").WaitAsync(DefaultTimeout);
    }

    public async Task DisposeAsync()
    {
        await Application.StopAsync();
        await Application.DisposeAsync();
    }
}