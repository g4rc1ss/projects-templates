using Projects;
#if (UseAspire)
using SimpleWeb.AspireHost.Resources;
#endif

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

#if (UsePostgres)
IResourceBuilder<PostgresDatabaseResource> postgres = builder.AddAspirePostgres();
#endif
#if (UseMongoDB)
IResourceBuilder<MongoDBServerResource> mongodb = builder.AddAspireMongo();
#endif
#if (UseRedis)
IResourceBuilder<RedisResource> redis = builder.AddRedisCache();
#endif
#if (UseGarnet)
IResourceBuilder<GarnetResource> garnet = builder.AddGarnetCache();
#endif

builder.AddProject<SimpleWeb_HostWebApi>("SimpleWeb")
#if (UsePostgres)
.WithAspirePostgres(postgres)
#endif
#if (UseMongoDB)
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
