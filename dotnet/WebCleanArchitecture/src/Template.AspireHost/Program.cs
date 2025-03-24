using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

#if (UsePostgres)
IResourceBuilder<ParameterResource> username = builder.AddParameter("username", "postgres");
IResourceBuilder<ParameterResource> password = builder.AddParameter("password", "123456");

IResourceBuilder<PostgresDatabaseResource> postgres = builder
    .AddPostgres("PostgresRN", username, password, 5432)
    .WithDataVolume("postgresVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("PostgresDB", "Template");
#endif

#if (UseRedis)
IResourceBuilder<RedisResource> redis = builder.AddRedis("Cache");
#endif

#if (UseGarnet)
IResourceBuilder<GarnetResource> garnet = builder.AddGarnet("Cache");
#endif


builder.AddProject<Template_HostWebApi>("Template")
#if (UsePostgres)
    .WithReference(postgres, "DatabaseContext")
    .WaitFor(postgres)
#endif
#if (UseRedis)
    .WithReference(redis)
    .WaitFor(redis)
#endif

#if (UseGarnet)
    .WithReference(garnet)
    .WaitFor(garnet)
#endif
    ;
builder.Build().Run();