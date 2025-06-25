 namespace SimpleWeb.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();

#if (UseGrpc)
        builder.Services.AddGrpcHealthChecks();
#endif
    }
}
