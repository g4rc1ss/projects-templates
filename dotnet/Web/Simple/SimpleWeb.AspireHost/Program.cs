using Projects;
#if (UseAspire)
using SimpleWeb.AspireHost.Resources;
#endif

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

#if (UsePostgres)
IResourceBuilder<PostgresServerResource> postgres = builder.AddAspirePostgres();
#endif
#if (UseMongodb)
IResourceBuilder<MongoDBServerResource> mongodb = builder.AddAspireMongo();
#endif
#if (UseRedis)
IResourceBuilder<RedisResource> redis = builder.AddRedisCache();
#endif
#if (UseGarnet)
IResourceBuilder<GarnetResource> garnet = builder.AddGarnetCache();
#endif

builder.AddProject<SimpleWeb_HostWebApi>("Host")
#if (UsePostgres)
.WithAspirePostgres(postgres)
#endif
#if (UseMongodb)
.WithAspireMongodb(mongodb)
#endif
#if (UseRedis)
.WithRedisCache(redis)
#endif
#if (UseGarnet)
.WithGarnetCache(garnet)
#endif
;

await builder.Build().RunAsync();
