using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> username = builder.AddParameter("username", "postgres");
IResourceBuilder<ParameterResource> password = builder.AddParameter("password", "123456");

#if (UsePostgres)
IResourceBuilder<PostgresDatabaseResource> postgres = builder
    .AddPostgres("PostgresRN", username, password, 5432)
    .WithDataVolume("postgresVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("PostgresDB", "RedNeuronal");
#endif
#if (UsePostgres)
builder.AddProject<Template_HostWebApi>("Template")
    .WithReference(postgres, "DatabaseContext")
    .WaitFor(postgres);
#else
builder.AddProject<Template_HostWebApi>("Template");
#endif

builder.Build().Run();