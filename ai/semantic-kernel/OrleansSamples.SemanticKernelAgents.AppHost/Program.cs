using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var openAiApiKey = builder.AddParameter("OpenAiApiKey");

var orleans = builder.AddOrleans("orleans")
    .WithDevelopmentClustering()
    .WithMemoryGrainStorage("Default");

builder.AddProject<Projects.OrleansSamples_SemanticKernelAgents_Silo>("silo")
    .WithReference(orleans)
    .WithEnvironment("OpenAI__ApiKey", openAiApiKey);

builder.Build().Run();
