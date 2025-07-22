using CreateDiscovery.Sample;
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new();

services.AddDependencies();

await using ServiceProvider provider = services.BuildServiceProvider();
IExample example = provider.GetRequiredService<IExample>();

example.Execute();
