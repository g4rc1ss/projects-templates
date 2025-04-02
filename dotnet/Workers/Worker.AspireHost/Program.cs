#if (UseAzServiceBus)
using Aspire.Hosting.Azure;
#endif
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

#if (UsePostgres)
IResourceBuilder<ParameterResource> username = builder.AddParameter("username", "postgres");
IResourceBuilder<ParameterResource> password = builder.AddParameter("password", "123456");

IResourceBuilder<PostgresDatabaseResource> postgres = builder
    .AddPostgres("Postgres", username, password, 5432)
    .WithDataVolume("postgresVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("PostgresDB", "WorkerTemplate");
#endif

#if (UseAzServiceBus)
IResourceBuilder<AzureServiceBusResource> azureServiceBus = builder.AddAzureServiceBus("AzureServiceBus");
azureServiceBus.RunAsEmulator();
#endif

#if (UseRabbitMQ)
IResourceBuilder<RabbitMQServerResource> rabbitMQ = builder.AddRabbitMQ("RabbitMQ")
    .WithDataVolume("rabbitMQVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .WithManagementPlugin();
#endif

#if (UseRedis)
IResourceBuilder<RedisResource> redis = builder.AddRedis("Cache");
#endif


IResourceBuilder<ProjectResource> project = builder.AddProject<WorkerTemplate>("WorkerTemplate");
#if (UsePostgres)
    project.WithReference(postgres, "DatabaseContext");
    project.WaitFor(postgres);
#endif

#if (UseRedis)
    project.WithReference(redis);
    project.WaitFor(redis);
#endif

#if (UseAzServiceBus)
    project.WithReference(azureServiceBus);
    project.WaitFor(azureServiceBus);
#endif

#if(UseRabbitMQ)
    project.WithReference(rabbitMQ);
    project.WaitFor(rabbitMQ);
#endif

await builder.Build().RunAsync();