using Projects;
#if (UseAzServiceBus)
using Aspire.Hosting.Azure;
#endif

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

#if (UsePostgres)
IResourceBuilder<ParameterResource> username = builder.AddParameter("username", "postgres");
IResourceBuilder<ParameterResource> password = builder.AddParameter("password", "123456");

IResourceBuilder<PostgresDatabaseResource> postgres = builder
    .AddPostgres("Postgres", username, password, 5432)
    .WithPgWeb()
    .WithDataVolume("postgresVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("PostgresDB", "Template");
#endif

#if (UseSqlServer || UseAzureSql)
IResourceBuilder<SqlServerDatabaseResource> sqlServer = builder
    .AddSqlServer("SQLServer")
    .WithDataVolume("SqlServerVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("TemplateDatabase", "Template");
#endif

#if (UseMongodb)
IResourceBuilder<MongoDBServerResource> mongoDb = builder
    .AddMongoDB("mongo")
    // .WithDataVolume("MongoVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .WithMongoExpress();
#endif

#if (UseAzServiceBus)
IResourceBuilder<AzureServiceBusResource> azureServiceBus = builder
    .AddAzureServiceBus("AzureServiceBus")
    .RunAsEmulator();
#endif

#if (UseRabbitMQ)
IResourceBuilder<RabbitMQServerResource> rabbitMQ = builder
    .AddRabbitMQ("RabbitMQ")
    .WithDataVolume("rabbitMQVM", isReadOnly: false)
    .WithLifetime(ContainerLifetime.Session)
    .WithManagementPlugin();
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

IResourceBuilder<ProjectResource> project = builder.AddProject<Template_HostWebApi>("Template");
#if (UsePostgres)
#if (UseIdentity)
project.WithReference(postgres, "IdentityDatabaseContext").WaitFor(postgres);
#else
project.WithReference(postgres, "DatabaseContext").WaitFor(postgres);
#endif
#endif

#if (UseSqlServer || UseAzureSql)
#if (UseIdentity)
project.WithReference(sqlServer, "IdentityDatabaseContext").WaitFor(sqlServer);
#else
project.WithReference(sqlServer, "DatabaseContext").WaitFor(sqlServer);
#endif
#endif

#if (UseMongodb)
project.WithReference(mongoDb).WaitFor(mongoDb);
#endif

#if (UseRedis)
project.WithReference(redis);
project.WaitFor(redis);
#endif

#if (UseGarnet)
project.WithReference(garnet).WaitFor(garnet);
#endif

#if (UseAzServiceBus)
project.WithReference(azureServiceBus).WaitFor(azureServiceBus);
#endif

#if (UseRabbitMQ)
project.WithReference(rabbitMQ).WaitFor(rabbitMQ);
#endif

await builder.Build().RunAsync();
