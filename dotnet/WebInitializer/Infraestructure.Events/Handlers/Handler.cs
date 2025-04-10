using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Infraestructure.Events.Handlers;

public record Request;

public class Handler(
    ILogger<Handler> logger,
    Channel<Request> channel
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            while (channel.Reader.TryRead(out Request? request))
            {
            }
        }
    }
}