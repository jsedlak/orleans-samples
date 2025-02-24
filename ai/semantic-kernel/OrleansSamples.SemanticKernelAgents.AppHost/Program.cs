var builder = DistributedApplication.CreateBuilder(args);

var orleans = builder.AddOrleans("orleans")
    .WithDevelopmentClustering()
    .WithMemoryGrainStorage("Default");

builder.AddProject<Projects.OrleansSamples_SemanticKernelAgents_Silo>("silo")
    .WithReference(orleans);

builder.Build().Run();
