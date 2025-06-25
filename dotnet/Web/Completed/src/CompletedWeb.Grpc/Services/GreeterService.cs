using CompletedWeb.Application.UsesCases;
using Grpc.Core;
using GrpcService1;
using Microsoft.Extensions.Logging;

namespace CompletedWeb.Grpc.Services;

public class GreeterService(
    ILogger<GreeterService> logger,
    IExample example) : Greeter.GreeterBase
{
    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        logger.LogInformation("Hello {RequestName}", request.Name);

        await example.ExecuteAsync(context.CancellationToken);

        return new HelloReply { Message = "Hello " + request.Name };
    }
}