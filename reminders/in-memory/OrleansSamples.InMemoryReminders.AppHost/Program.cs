var builder = DistributedApplication.CreateBuilder(args);

var orleans = builder.AddOrleans("orleans")
    .WithMemoryReminders()
    .WithMemoryGrainStorage("Default")
    .WithDevelopmentClustering();

builder.AddProject<Projects.OrleansSamples_InMemoryReminders_Silo>("silo")
    .WithReference(orleans);

builder.Build().Run();
