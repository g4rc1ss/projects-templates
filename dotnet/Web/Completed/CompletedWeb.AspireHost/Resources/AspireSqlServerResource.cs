namespace CompletedWeb.AspireHost.Resources;

public static class AspireSqlServerResource
{
    internal static IResourceBuilder<SqlServerDatabaseResource> AddAspireSqlServer(
        this IDistributedApplicationBuilder builder
    )
    {
        IResourceBuilder<ParameterResource> password = builder.AddParameter(
            "sqlServerPass",
            "xZgrnjfdnru2342"
        );

        IResourceBuilder<SqlServerServerResource> sqlServer = builder
            .AddSqlServer("MSqlServer", password, 1433)
            // .WithDataVolume("SqlServerVM", isReadOnly: false)
            .WithLifetime(ContainerLifetime.Session);

        return sqlServer.AddDatabase("SqlServer", "MicrosoftDatabase");
    }

    internal static IResourceBuilder<T> WithAspireSqlServer<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<SqlServerDatabaseResource> sqlServer
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(sqlServer).WaitFor(sqlServer);
    }
}
