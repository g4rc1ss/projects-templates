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

#if (UseMongodb)
IResourceBuilder<MongoDBDatabaseResource> mongoDb = builder.AddMongoDB("mongodb")
    .WithDataVolume("MongoVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("WorkerTemplate");
#endif

IResourceBuilder<ProjectResource> project = builder.AddProject<WorkerTemplate>("WorkerTemplate");
#if (UsePostgres)
project.WithReference(postgres, "DatabaseContext")
    .WaitFor(postgres);
#endif

#if (UseMongodb)
project.WithReference(mongoDb)
    .WaitFor(mongoDb);
#endif

#if (UseRedis)
project.WithReference(redis)
    .WaitFor(redis);
#endif

#if (UseAzServiceBus)
project.WithReference(azureServiceBus)
    .WaitFor(azureServiceBus);
#endif

#if(UseRabbitMQ)
project.WithReference(rabbitMQ)
    .WaitFor(rabbitMQ);
#endif

await builder.Build().RunAsync();