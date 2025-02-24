var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");

// TODO: Add Redis Streaming Provider
var orleans = builder.AddOrleans("orleans")
    .WithClustering(redis)
    .WithGrainStorage("Default", redis)
    .WithGrainStorage("PubSubStore", redis)
    .WithReminders(redis)
    .WithMemoryStreaming("StreamProvider");

builder.AddProject<Projects.OrleansSamples_RedisProviders_Silo>("silo")
    .WithReference(orleans)
    .WithReference(redis)
    .WaitFor(redis);

builder.Build().Run();
