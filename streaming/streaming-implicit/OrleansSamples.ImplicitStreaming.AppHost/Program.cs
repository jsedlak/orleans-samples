var builder = DistributedApplication.CreateBuilder(args);

var orleans = builder.AddOrleans("orleans")
    .WithDevelopmentClustering()
    .WithMemoryGrainStorage("PubSubStore")
    .WithMemoryGrainStorage("Default")
    .WithMemoryStreaming("StreamProvider");

builder.AddProject<Projects.OrleansSamples_ImplicitStreaming_Silo>("silo")
    .WithReference(orleans);

builder.Build().Run();
