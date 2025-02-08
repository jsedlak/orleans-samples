var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("reminders");

var orleans = builder
    .AddOrleans("orleans")
    .WithDevelopmentClustering()
    .WithMemoryGrainStorage("Default")
    .WithReminders(redis);

builder.AddProject<Projects.OrleansSamples_RedisReminders_Silo>("silo")
    .WithReference(redis)
    .WithReference(orleans);

builder.Build().Run();
