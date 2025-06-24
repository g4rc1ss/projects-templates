using Projects;
#if (UseAspire)
using CompletedWeb.AspireHost.Resources;
#endif
#if (UseAzureStorage)
using Aspire.Hosting.Azure;
using Microsoft.Extensions.Azure;
#endif

#if (UseAzServiceBus)
using Aspire.Hosting.Azure;
#endif

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

#if (UsePostgres)
IResourceBuilder<PostgresDatabaseResource> postgres = builder.AddAspirePostgres();
#endif
#if (UseSqlServer || UseAzureSql)
IResourceBuilder<SqlServerDatabaseResource> sqlServer = builder.AddAspireSqlServer();
#endif
#if (UseMongodb)
IResourceBuilder<MongoDBServerResource> mongodb = builder.AddAspireMongo();
#endif
#if (UseAzServiceBus)
IResourceBuilder<AzureServiceBusResource> azureServiceBus = builder.AddAspireAzureServiceBus();
#endif
#if (UseRabbitMQ)
IResourceBuilder<RabbitMQServerResource> rabbitMQ = builder.AddAspireRabbitMq();
#endif
#if (UseRedis)
IResourceBuilder<RedisResource> redis = builder.AddRedisCache();
#endif
#if (UseGarnet)
IResourceBuilder<GarnetResource> garnet = builder.AddGarnetCache();
#endif
#if (UseAzureStorage)
IResourceBuilder<AzureBlobStorageResource> blobStorage = builder.AddAspireAzBlobStorage();
#endif

builder.AddProject<CompletedWeb_HostWebApi>(
        "CompletedWeb"
    )
#if (UsePostgres)
.WithAspirePostgres(postgres)
#endif
#if (UseSqlServer || UseAzureSql)
.WithAspireSqlServer(sqlServer)
#endif
#if (UseMongodb)
.WithAspireMongodb(mongodb)
#endif
#if (UseAzServiceBus)
.WithAspireAzServiceBus()
#endif
#if (UseRabbitMQ)
.WithAspireRabbitMq(rabbitMQ)
#endif
#if (UseRedis)
.WithRedisCache(redis)
#endif
#if (UseGarnet)
.WithGarnetCache(garnet)
#endif
#if (UseAzureStorage)
.WithAspireRabbitMq(blobStorage)
#endif
    ;

await builder.Build().RunAsync();