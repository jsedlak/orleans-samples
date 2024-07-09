// See https://aka.ms/new-console-template for more information

using ImplicitStreams.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
    .UseOrleansClient((context, client) => { client.UseLocalhostClustering(); })
    .UseConsoleLifetime()
    .Build();

await host.StartAsync();

var client = host.Services.GetRequiredService<IClusterClient>();

var id = Guid.NewGuid();
var producer = client.GetGrain<IProducerGrain>(id);
await producer.StartProducing();

Console.ReadKey();

await host.StopAsync();