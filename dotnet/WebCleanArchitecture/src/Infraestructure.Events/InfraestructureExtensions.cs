using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.Events;

public static class InfraestructureEventsExtensions
{
    public static void AddEventsServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(InfraestructureEventsExtensions).Assembly);
    }
}
