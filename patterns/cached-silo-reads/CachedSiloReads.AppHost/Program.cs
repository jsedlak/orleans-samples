var builder = DistributedApplication.CreateBuilder(args);

var orleans = builder.AddOrleans("orleans")
    .WithDevelopmentClustering()
    .WithMemoryGrainStorage("Default")
    .WithMemoryGrainStorage("PubSubStore")
    .WithMemoryReminders()
    .WithMemoryStreaming("StreamProvider");

builder.AddProject<Projects.CachedSiloReads_SiloHost>("silo")
    .WithReference(orleans);

builder.Build().Run();
