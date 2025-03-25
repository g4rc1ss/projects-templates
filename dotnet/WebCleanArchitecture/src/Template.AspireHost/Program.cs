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
    .AddDatabase("PostgresDB", "Template");
#endif

#if (UseAzServiceBus)
IResourceBuilder<AzureServiceBusResource> azureServiceBus = builder.AddAzureServiceBus("AzureServiceBus");
azureServiceBus.AddServiceBusQueue("serviceBusQueue");
azureServiceBus.AddServiceBusTopic("serviceBusTopic");
azureServiceBus.RunAsEmulator();
#elif (UseRabbitMQ)
IResourceBuilder<RabbitMQServerResource> rabbitMQ = builder.AddRabbitMQ("RabbitMQ")
    .WithDataVolume("rabbitMQVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .WithManagementPlugin();
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
#elif (UseGarnet)
    .WithReference(garnet)
    .WaitFor(garnet)
#endif
#if (UseAzServiceBus)
    .WithReference(azureServiceBus)
    .WaitFor(azureServiceBus)
#elif (UseRabbitMQ)
    .WithReference(rabbitMQ)
    .WaitFor(rabbitMQ)
#endif
    ;
builder.Build().Run();