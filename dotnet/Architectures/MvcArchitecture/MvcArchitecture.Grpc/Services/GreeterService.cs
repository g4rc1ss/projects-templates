using Grpc.Core;
using GrpcService1;
using Microsoft.Extensions.Logging;

namespace MvcArchitecture.Grpc.Services;

public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        logger.LogInformation("SayHello called");
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }
}
