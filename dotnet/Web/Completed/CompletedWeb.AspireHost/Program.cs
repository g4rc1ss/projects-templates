using Projects;
#if (UseAspire)
using CompletedWeb.AspireHost.Resources;
#endif

#if (UseAzureBlobStorage)
using Microsoft.Extensions.Azure;
#endif

#if (UseAzServiceBus || UseAzureCosmos || UseAzureBlobStorage)
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
#if (UseAzureCosmos)
IResourceBuilder<AzureCosmosDBDatabaseResource> cosmosdb = builder.AddAspireAzureCosmos();
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
#if (UseAzureBlobStorage)
IResourceBuilder<AzureBlobStorageResource> blobStorage = builder.AddAspireAzBlobStorage();
#endif

builder.AddProject<CompletedWeb_HostWebApi>("CompletedWeb")
#if (UsePostgres)
.WithAspirePostgres(postgres)
#endif
#if (UseSqlServer || UseAzureSql)
.WithAspireSqlServer(sqlServer)
#endif
#if (UseMongodb)
.WithAspireMongodb(mongodb)
#endif
#if (UseAzureCosmos)
.WithAspireAzureCosmos(cosmosdb)
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
#if (UseAzureBlobStorage)
.WithAspireAzBlobStorage(blobStorage)
#endif
;

await builder.Build().RunAsync();
