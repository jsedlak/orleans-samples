using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using SatellitePattern.Shared;

var host = Host.CreateDefaultBuilder()
    .UseOrleans(silo =>
    {
        silo.UseLocalhostClustering()
            .AddMemoryGrainStorageAsDefault()
            .AddMemoryStreams(Constants.StreamProvider, configurator => configurator.ConfigureStreamPubSub(StreamPubSubType.ImplicitOnly));
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();