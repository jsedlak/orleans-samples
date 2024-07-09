// See https://aka.ms/new-console-template for more information

using ImplicitStreams.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Hosting;
using Orleans.Streams;

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