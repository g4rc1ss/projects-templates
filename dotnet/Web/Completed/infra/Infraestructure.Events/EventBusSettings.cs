#if (UseMemoryEvents)
using System.Reflection;
#endif

namespace Infraestructure.Events;

public class EventBusSettings
{
    public bool DisableTracing { get; init; }
#if (UseMemoryEvents)
    public IEnumerable<Assembly> Assemblies { get; init; }
#endif
};
