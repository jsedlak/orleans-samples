using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var orleans = builder.AddOrleans("orleans")
    .WithDevelopmentClustering()
    .WithMemoryGrainStorage("Default")
    .WithMemoryGrainStorage("PubSubStore")
    .WithMemoryStreaming("StreamProvider");

var silo = builder.AddProject<OrleansSamples_Patterns_SatellitePattern_Silo>("silo")
    .WithReference(orleans);

builder.Build().Run();