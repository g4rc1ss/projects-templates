using Grpc.Core;
using GrpcService1;
using Microsoft.Extensions.Logging;

namespace CompletedWeb.Grpc.Services;

public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        logger.LogInformation("Hello {RequestName}", request.Name);
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }
}
