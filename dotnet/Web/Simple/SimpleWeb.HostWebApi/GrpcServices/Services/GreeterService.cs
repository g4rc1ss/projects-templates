using Grpc.Core;
using GrpcService1;
using SimpleWeb.HostWebApi.UsesCases;

namespace SimpleWeb.HostWebApi.GrpcServices.Services;

public class GreeterService(Example example, ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        logger.LogInformation("SayHello called");

        await example.ExecuteAsync();

        return new HelloReply { Message = "Hello " + request.Name };
    }
}
