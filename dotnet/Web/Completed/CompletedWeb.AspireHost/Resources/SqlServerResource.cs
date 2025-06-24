namespace CompletedWeb.AspireHost.Resources;

public static class SqlServerResource
{
    internal static IResourceBuilder<SqlServerDatabaseResource> AddAspireSqlServer(
        this IDistributedApplicationBuilder builder
    )
    {
        return builder
            .AddSqlServer("SQLServer")
            .WithDataVolume("SqlServerVM", isReadOnly: false)
            .WithLifetime(ContainerLifetime.Session)
            .AddDatabase("CompletedWebDatabase", "CompletedWeb");
    }

    internal static IResourceBuilder<T> WithAspireSqlServer<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<SqlServerDatabaseResource> sqlServer
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
#if (UseIdentity)
return builder.WithReference(sqlServer, "IdentityDatabaseContext").WaitFor(sqlServer);
#else
        return builder.WithReference(sqlServer, "DatabaseContext").WaitFor(sqlServer);
#endif
    }
}