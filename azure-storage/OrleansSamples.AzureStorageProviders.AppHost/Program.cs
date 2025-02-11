var builder = DistributedApplication.CreateBuilder(args);

var storage = builder
    .AddAzureStorage("azure-storage")
    .RunAsEmulator(r => r.WithImage("azure-storage/azurite", "3.33.0"));

var clustering = storage.AddTables("clustering");
var reminders = storage.AddTables("reminders");
var streaming = storage.AddQueues("streaming");
var grainStore = storage.AddBlobs("grain-storage");
var pubSubStore = storage.AddBlobs("pubsub-storage");

var orleans = builder.AddOrleans("orleans")
    .WithClustering(clustering)
    .WithReminders(reminders)
    .WithStreaming("StreamProvider", streaming)
    .WithGrainStorage("Default", grainStore)
    .WithGrainStorage("PubSubStore", pubSubStore);

builder.AddProject<Projects.OrleansSamples_AzureStorageProviders_Silo>("silo")
    .WithReference(orleans)
    .WithReference(clustering)
    .WaitFor(clustering)
    .WithReference(reminders)
    .WaitFor(reminders)
    .WithReference(streaming)
    .WaitFor(streaming)
    .WithReference(grainStore)
    .WaitFor(grainStore)
    .WithReference(pubSubStore)
    .WaitFor(pubSubStore);

builder.Build().Run();
