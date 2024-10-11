using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SatellitePattern.Shared.Actors;

var host = Host.CreateDefaultBuilder()
    .UseOrleansClient((context, client) => { client.UseLocalhostClustering(); })
    .UseConsoleLifetime()
    .Build();

await host.StartAsync();

// generate a random account id
var accountId = Guid.NewGuid();

// get access to the cluster
var client = host.Services.GetRequiredService<IClusterClient>();

/*
 * The Event Driven Satellite uses a Stream to receive events from the Account Actor
 * and stores them in a database for optimizing querying across all accounts.
 * 
 * This sample does not provide a database, but does demonstrate how the Streaming
 * would be implemented using Implicit Streams.
 */
var eventDrivenAccount = client.GetGrain<IEventDrivenAccountActor>(accountId);
await eventDrivenAccount.SignIn("Playing Solitaire");

Console.WriteLine("Pretending to read from the database (our satellite)...");

/*
 * The "Standard" Satellite Grain method uses a direct reference to the Satellite Grain
 * and sets the internal state of that Grain using Grain-to-Grain communication. This
 * provides an easy way to get started with satellite grains, but exposes the ability
 * to set the status to callers as demonstrated below.
 */
var account = client.GetGrain<IAccountActor>(accountId);
await account.SignIn("Playing Mahjong");

var accountSatellite = client.GetGrain<IAccountSatelliteActor>(accountId);

// oops! we can set the status mistakenly!
await accountSatellite.SetStatus(null); 

var status = await accountSatellite.GetStatus();
Console.WriteLine($"Account Status: {status.ToDisplayString()}");

/*
 * By using Grain Extensions, the caller cannot directly set the state of the Satellite
 * Grain, but can still interact with it. This provides a more secure way to interact
 * with the state and builds a bridge between the Account and Satellite Grains.
 * 
 * This setup is ideal for when you want to query the state of the Account on an
 * individual basis and receive a fast and up-to-date response.
 */
var secureAccount = client.GetGrain<ISecureAccountActor>(accountId);
await secureAccount.SignIn("Playing Counter Strike");

var secureSatellite = client.GetGrain<IAccountSecureSatelliteActor>(accountId);
var secureStatus = await secureSatellite.GetStatus();
Console.WriteLine($"Secure Account Status: {secureStatus.ToDisplayString()}");
