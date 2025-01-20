var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
                            .WithImage("dddanielreis/orleans-postgres")
                            .WithPgAdmin();

var orleans = builder.AddOrleans("orleans")
    .WithClustering(postgres)
    .WithMemoryGrainStorage("Default");

builder.AddProject<Projects.OrleansSamples_PostgresClustering_Silo>("silo")
    .WithReference(orleans)
    .WithReference(postgres)
    .WaitFor(postgres);

builder.Build().Run();
