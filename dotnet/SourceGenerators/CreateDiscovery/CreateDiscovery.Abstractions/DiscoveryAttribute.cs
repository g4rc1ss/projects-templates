namespace CreateDiscovery.Abstractions;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class DiscoveryAttribute : Attribute
{
    public LifeTime Lifetime { get; set; }
}
