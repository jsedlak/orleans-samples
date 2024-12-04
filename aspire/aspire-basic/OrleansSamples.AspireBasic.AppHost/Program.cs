using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var orleans = builder
    .AddOrleans("aspire-basic-orleans")
    .WithDevelopmentClustering()
    .WithMemoryGrainStorage("Default");

builder.AddProject<Projects.OrleansSamples_AspireBasic_Silo>("aspire-basic-silo")
    .WithReference(orleans);

builder.Build().Run();
