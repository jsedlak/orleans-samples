using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GzipSample.Shared;
using Orleans.Storage;
using Orleans.Serialization;

var host = Host.CreateDefaultBuilder()
    .UseOrleans(silo =>
    {
        silo.UseLocalhostClustering()
            .AddMemoryGrainStorageAsDefault();
    })
    .ConfigureServices(services =>
    {
        services.AddKeyedSingleton<IGrainStorageSerializer, JsonGrainStorageSerializer>("TestProvider");
        services.AddGzipSerializer("TestProvider");

        //services.AddSerializer(builder =>
        //{
        //    builder.AddJsonSerializer(isSupported: type => true);
        //});
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();