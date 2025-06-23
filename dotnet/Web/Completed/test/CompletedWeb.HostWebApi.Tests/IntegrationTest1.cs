namespace CompletedWeb.HostWebApi.Tests;

public class IntegrationTest1(ApiServiceFixture fixture) : IClassFixture<ApiServiceFixture>
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCodeAsync()
    {
        // Arrange
        HttpClient httpClient = fixture.Application!.CreateHttpClient("CompletedWeb");

        // Act
        HttpResponseMessage response = await httpClient.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
