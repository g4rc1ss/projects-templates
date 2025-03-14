using Microsoft.Extensions.Hosting;

namespace Functionality.Infraestructure.Test;

public class IntegrationTest1(
    InfraestructureFixture infra
) : IClassFixture<InfraestructureFixture>
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        IHostEnvironment? environment = infra.Application.Services.GetService<IHostEnvironment>();
    }
}