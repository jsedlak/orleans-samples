var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("storage");

var orleans = builder.AddOrleans("orleans")
    .WithDevelopmentClustering()
    .WithGrainStorage("Default", redis);

builder.AddProject<Projects.OrleansSamples_RedisStorage_Silo>("silo")
    .WithReference(orleans)
    .WithReference(redis)
    .WaitFor(redis);

builder.Build().Run();
