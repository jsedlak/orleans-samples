var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("clustering");

var orleans = builder
    .AddOrleans("orleans")
    .WithClustering(redis)
    .WithMemoryGrainStorage("Default");

builder.AddProject<Projects.OrleansSamples_RedisClustering_Silo>("silo")
    .WithReplicas(3)
    .WithReference(orleans)
    .WithReference(redis)
    .WaitFor(redis);

builder.Build().Run();
