namespace HostTestConfiguration;

public class WebHostTestConfig
{
    public HttpClient Client { get; set; }
    public IServiceProvider ServiceProvider { get; set; }

    public WebHostTestConfig()
    {
        WebApplicationFactoryHost webFactory = new();
        ServiceProvider = webFactory.Services;
        Client = webFactory.CreateClient();
    }
}