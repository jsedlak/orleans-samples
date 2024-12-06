var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(r => r.WithImage("azure-storage/azurite", "3.33.0"));

var grainStorage = storage.AddBlobs("GrainStorage");

var orleans = builder.AddOrleans("orleans")
    .WithDevelopmentClustering()
    .WithGrainStorage("Default", grainStorage);

builder.AddProject<Projects.OrleansSamples_GrainStorage_AzureBlobs_Silo>("silo")
    .WithReference(orleans)
    .WithReference(grainStorage)
    .WaitFor(grainStorage);

builder.Build().Run();
