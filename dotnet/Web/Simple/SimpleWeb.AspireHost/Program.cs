using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

#if (UsePostgres)
IResourceBuilder<ParameterResource> username = builder.AddParameter("username", "postgres");
IResourceBuilder<ParameterResource> password = builder.AddParameter("password", "123456");

IResourceBuilder<PostgresDatabaseResource> postgres = builder
    .AddPostgres("Postgres", username, password, 5432)
    .WithPgWeb()
    .WithDataVolume("postgresVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("PostgresDB", "SimpleWeb");
#endif

#if (UseRedis)
IResourceBuilder<RedisResource> redis = builder
    .AddRedis("Cache")
    .WithRedisCommander()
    .WithRedisInsight();
#endif

#if (UseGarnet)
IResourceBuilder<GarnetResource> garnet = builder.AddGarnet("Cache");
#endif

IResourceBuilder<ProjectResource> project = builder.AddProject<SimpleWeb_HostWebApi>(
    "SimpleWeb"
);
#if (UsePostgres)
project.WithReference(postgres, "DatabaseContext").WaitFor(postgres);
#endif

#if (UseRedis)
project.WithReference(redis);
project.WaitFor(redis);
#endif

#if (UseGarnet)
project.WithReference(garnet).WaitFor(garnet);
#endif

await builder.Build().RunAsync();