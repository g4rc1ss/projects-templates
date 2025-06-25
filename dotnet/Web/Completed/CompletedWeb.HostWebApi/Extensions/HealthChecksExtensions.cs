namespace CompletedWeb.HostWebApi.Extensions;

public static class HealthChecksExtensions
{
    public static void AddHealthChecks(this IHostApplicationBuilder builder)
    {
#if (UseApi)
        builder.Services.AddHealthChecks();
#endif

#if (UseGrpc)
        builder.Services.AddGrpcHealthChecks();
#endif
    }
}
