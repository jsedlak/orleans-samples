var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres-server")
                            .WithImage("dddanielreis/orleans-postgres")
                            .WithPgAdmin();

var clusteringDb = postgres.AddDatabase("postgres");

var orleans = builder.AddOrleans("orleans")
    .WithClustering(clusteringDb)
    .WithMemoryGrainStorage("Default");

builder.AddProject<Projects.OrleansSamples_PostgresClustering_Silo>("silo")
    .WithReference(orleans)
    .WithReference(postgres)
    .WaitFor(postgres);

builder.Build().Run();
