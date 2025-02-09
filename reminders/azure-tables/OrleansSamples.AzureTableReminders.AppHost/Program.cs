var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

var reminders = storage.AddTables("reminders");

var orleans = builder
    .AddOrleans("orleans")
    .WithDevelopmentClustering()
    .WithReminders(reminders)
    .WithMemoryGrainStorage("Default");

builder.AddProject<Projects.OrleansSamples_AzureTableReminders_Silo>("silo")
    .WithReference(orleans)
    .WithReference(reminders)
    .WaitFor(reminders);

builder.Build().Run();
