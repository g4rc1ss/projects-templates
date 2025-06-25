using Grpc.Core;
using GrpcService1;
#if (SqlDatabase || NoSqlDatabase)
using SimpleWeb.HostWebApi.UsesCases;
#endif

namespace SimpleWeb.HostWebApi.GrpcServices.Services;

public class GreeterService(
#if (SqlDatabase || NoSqlDatabase)
    Example example,
#endif
    ILogger<GreeterService> logger) : Greeter.GreeterBase
{
    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        logger.LogInformation("SayHello called");

#if (UsePostgres)
        await example.PostgresAsync();
#endif
#if (UseSqlite)
        await example.SqliteAsync();
#endif
#if (UseMongodb)
        await example.MongodbAsync();
#endif
#if (UseLitedb)
        await example.LitedbAsync();
#endif
        return new HelloReply { Message = "Hello " + request.Name };
    }
}
