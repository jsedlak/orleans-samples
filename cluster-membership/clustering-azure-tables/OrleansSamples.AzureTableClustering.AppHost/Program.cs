var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(r => r.WithImage("azure-storage/azurite", "3.33.0"));

var clusteringTable = storage.AddTables("clustering");

var orleans = builder
    .AddOrleans("orleans")
    .WithClustering(clusteringTable)
    .WithMemoryGrainStorage("Default");

builder.AddProject<Projects.OrleansSamples_AzureTableClustering_Silo>("silo")
    .WithReference(orleans)
    .WithReplicas(3)
    .WithReference(clusteringTable)
    .WaitFor(clusteringTable);

builder.Build().Run();
