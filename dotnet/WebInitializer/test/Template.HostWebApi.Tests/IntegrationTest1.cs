namespace Template.HostWebApi.Tests;

public class IntegrationTest1(ApiServiceFixture fixture) : IClassFixture<ApiServiceFixture>
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
    {
        // Arrange
        HttpClient httpClient = fixture.Application!.CreateHttpClient("Template");

        // Act
        HttpResponseMessage response = await httpClient.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
