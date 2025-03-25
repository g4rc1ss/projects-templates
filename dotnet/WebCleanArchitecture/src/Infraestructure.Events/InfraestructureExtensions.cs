#if (UseMemoryEvents)
using MediatR;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.Events;

public static class InfraestructureEventsExtensions
{
    public static void AddEventsServices(this IServiceCollection services,
        IConfiguration configuration)
    {
#if (UseMemoryEvents)
        services.AddMediatR(typeof(InfraestructureEventsExtensions).Assembly);
#endif
    }
}