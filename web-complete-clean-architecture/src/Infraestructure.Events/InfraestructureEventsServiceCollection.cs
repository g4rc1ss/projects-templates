using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.Events;

public static class InfraestructureEventsServiceCollection
{
    public static void AddEventsServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(InfraestructureEventsServiceCollection).Assembly);
    }
}
