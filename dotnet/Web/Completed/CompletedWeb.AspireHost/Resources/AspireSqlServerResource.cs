namespace CompletedWeb.AspireHost.Resources;

public static class AspireSqlServerResource
{
    internal static IResourceBuilder<SqlServerServerResource> AddAspireSqlServer(
        this IDistributedApplicationBuilder builder
    )
    {
        IResourceBuilder<SqlServerServerResource> sqlServer = builder
            .AddSqlServer("SqlServer")
            .WithDataVolume("SqlServerVM", isReadOnly: false)
            .WithLifetime(ContainerLifetime.Session);

        sqlServer.AddDatabase("SqlServerDatabase", "MicrosoftDatabase");

        return sqlServer;
    }

    internal static IResourceBuilder<T> WithAspireSqlServer<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<SqlServerServerResource> sqlServer
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
#if (UseIdentity)
        return builder.WithReference(sqlServer).WaitFor(sqlServer);
#else
        return builder.WithReference(sqlServer).WaitFor(sqlServer);
#endif
    }
}
