using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using SatellitePattern.Data;
using SatellitePattern.Shared;
using SatellitePattern.Shared.Services;

var host = Host.CreateDefaultBuilder()
    .UseOrleans(silo =>
    {
        silo.UseLocalhostClustering()
            .AddMemoryGrainStorageAsDefault()
            .AddMemoryStreams(Constants.StreamProvider, configurator => configurator.ConfigureStreamPubSub(StreamPubSubType.ImplicitOnly));
    })
    .ConfigureServices(services =>
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("SatellitePatternData"));
        services.AddSingleton<IAccountStatusService, EntityFrameworkAccountStatusService>();
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime()
    .Build();

await host.RunAsync();