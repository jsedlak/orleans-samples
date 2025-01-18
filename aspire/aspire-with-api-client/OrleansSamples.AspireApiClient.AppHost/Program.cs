var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(r => r.WithImage("azure-storage/azurite", "3.33.0"));

var clusteringTable = storage.AddTables("clustering");

var orleans = builder
    .AddOrleans("aspire-api-orleans")
    .WithClustering(clusteringTable)
    .WithMemoryGrainStorage("Default");

var silo = builder.AddProject<Projects.OrleansSamples_AspireApiClient_Silo>("orleanssamples-aspireapiclient-silo")
    .WithReference(orleans)
    .WaitFor(storage);

builder.AddProject<Projects.OrleansSamples_AspireApiClient_Api>("orleanssamples-aspireapiclient-api")
    .WithReference(orleans.AsClient())
    .WaitFor(silo);

builder.Build().Run();
