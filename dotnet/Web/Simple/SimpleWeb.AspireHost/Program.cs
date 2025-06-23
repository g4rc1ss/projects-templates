using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> username = builder.AddParameter("username", "postgres");
IResourceBuilder<ParameterResource> password = builder.AddParameter("password", "123456");

IResourceBuilder<PostgresDatabaseResource> postgres = builder
    .AddPostgres("Postgres", username, password, 5432)
    .WithPgWeb()
    .WithDataVolume("postgresVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("PostgresDB", "SimpleWeb");





IResourceBuilder<RedisResource> redis = builder
    .AddRedis("Cache")
    .WithRedisCommander()
    .WithRedisInsight();


IResourceBuilder<ProjectResource> project = builder.AddProject<SimpleWeb_HostWebApi>(
    "SimpleWeb"
);
project.WithReference(postgres, "DatabaseContext").WaitFor(postgres);



project.WithReference(redis);
project.WaitFor(redis);




await builder.Build().RunAsync();
