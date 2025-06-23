using Projects;

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

IResourceBuilder<ProjectResource> project = builder.AddProject<SimpleWeb_HostWebApi>("SimpleWeb");
#if (UsePostgres)
project.WithAspirePostgres(postgres);
#endif
#if (UseMongoDB)
project.WithAspireMongodb(mongodb);
#endif
#if (UseRedis)
project.WithRedisCache(redis);
#endif
#if (UseGarnet)
project.WithReference(garnet).WaitFor(garnet);
#endif

await builder.Build().RunAsync();
