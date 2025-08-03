namespace SimpleWeb.AspireHost.Resources;

public static class AspirePostgresResource
{
    internal static IResourceBuilder<PostgresDatabaseResource> AddAspirePostgres(
        this IDistributedApplicationBuilder builder
    )
    {
        IResourceBuilder<ParameterResource> username = builder.AddParameter("username", "postgres");
        IResourceBuilder<ParameterResource> password = builder.AddParameter("password", "123456");

        IResourceBuilder<PostgresServerResource> postgres = builder
            .AddPostgres("PostgresServer", username, password, 5432)
            .WithPgWeb()
            // .WithDataVolume("postgresVM", isReadOnly: false)
            .WithLifetime(ContainerLifetime.Session);

        return postgres.AddDatabase("Postgres", "PostgresDatabase");
    }

    internal static IResourceBuilder<T> WithAspirePostgres<T>(
        this IResourceBuilder<T> builder,
        IResourceBuilder<PostgresDatabaseResource> postgres
    )
        where T : IResourceWithWaitSupport, IResourceWithEnvironment
    {
        return builder.WithReference(postgres).WaitFor(postgres);
    }
}
