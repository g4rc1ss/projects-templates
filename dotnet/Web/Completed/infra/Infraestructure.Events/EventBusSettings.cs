#if (UseMemoryEvents)
using System.Reflection;
#endif

namespace Infraestructure.Events;

public class EventBusSettings
{
    public bool DisableTracing { get; set; }
#if (UseMemoryEvents)
    public IEnumerable<Assembly>? Assemblies { get; set; }
#endif
};
