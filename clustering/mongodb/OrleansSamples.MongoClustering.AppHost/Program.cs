var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo");
var clustering = mongo.AddDatabase("clustering");

var orleans = builder.AddOrleans("orleans")
    .WithClustering(clustering)
    .WithMemoryGrainStorage("Default");

builder.AddProject<Projects.OrleansSamples_MongoClustering_Silo>("silo")
    .WithReplicas(1)
    .WithReference(orleans)
    .WithReference(mongo)
    .WaitFor(mongo);

builder.Build().Run();
