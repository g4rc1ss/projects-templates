using CreateDiscovery.Abstractions;

namespace CreateDiscovery.Sample;

[Discovery(Lifetime = LifeTime.SCOPED)]
public class ExampleService : IExample
{
    public void Execute()
    {
        Console.WriteLine("Creating discovery generator");
    }
}

public interface IExample
{
    void Execute();
}
