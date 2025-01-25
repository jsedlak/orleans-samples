var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(r => r.WithImage("azure-storage/azurite", "3.33.0"));

var clusteringTable = storage.AddTables("clustering");
var grainStorage = storage.AddBlobs("grain-storage");

var orleans = builder
    .AddOrleans("aspire-api-orleans")
    .WithClustering(clusteringTable)
    .WithGrainStorage("Default", grainStorage);

var silo = builder.AddProject<Projects.OrleansSamples_AspireApiClient_Silo>("silo")
    .WithReference(orleans)
    .WithReference(clusteringTable)
    .WithReference(grainStorage)
    .WaitFor(grainStorage)
    .WaitFor(clusteringTable);

builder.AddProject<Projects.OrleansSamples_AspireApiClient_Api>("api")
    .WithReference(orleans.AsClient())
    .WithReference(clusteringTable)
    .WaitFor(silo);

builder.Build().Run();
