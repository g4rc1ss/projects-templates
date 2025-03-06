using HostTestConfiguration;

namespace Template.API.Test;

[Collection(Ids.IDENTIFICADOR_WEB_HOST)]
public class UnitTest1(WebHostTestConfig fixture)
{
    [Fact]
    public async Task Test1()
    {
        string response = await fixture.Client.GetStringAsync("api/template");

        Assert.NotNull(response);
        Assert.NotEmpty(response);
    }
}