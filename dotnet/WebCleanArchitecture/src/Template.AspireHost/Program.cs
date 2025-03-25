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
#elif (UseSqlServer || UseAzureSql)
IResourceBuilder<SqlServerDatabaseResource> sqlServer = builder
    .AddSqlServer("SQLServer")
    .WithDataVolume("SqlServerVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("TemplateDatabase", "Template");
#endif

#if (UseAzServiceBus)
IResourceBuilder<AzureServiceBusResource> azureServiceBus = builder.AddAzureServiceBus("AzureServiceBus");
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


IResourceBuilder<ProjectResource> project = builder.AddProject<Template_HostWebApi>("Template");
#if (UsePostgres)
    project.WithReference(postgres, "DatabaseContext");
    project.WaitFor(postgres);
#elif (UseSqlServer || UseAzureSql)
    project.WithReference(sqlServer, "DatabaseContext");
    project.WaitFor(sqlServer);
#endif
#if (UseRedis)
    project.WithReference(redis);
    project.WaitFor(redis);
#elif (UseGarnet)
    project.WithReference(garnet);
    project.WaitFor(garnet);
#endif
#if (UseAzServiceBus)
    project.WithReference(azureServiceBus);
    project.WaitFor(azureServiceBus);
#elif (UseRabbitMQ)
    project.WithReference(rabbitMQ);
    project.WaitFor(rabbitMQ);
#endif
builder.Build().Run();