using GzipSample.Shared.Actors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
    .UseOrleansClient((context, client) => { client.UseLocalhostClustering(); })
    .UseConsoleLifetime()
    .Build();

await host.StartAsync();

// get access to the cluster
var client = host.Services.GetRequiredService<IClusterClient>();

var accountId = Random.Shared.Next(1, 1000);

var account = client.GetGrain<IBankAccount>(accountId);

Console.WriteLine("Depositing some money.");
await account.Deposit(Random.Shared.NextDouble() * 2_000);
await account.Deposit(Random.Shared.NextDouble() * 10_000);

var balance = await account.CheckBalance();
Console.WriteLine($"Available balance: {balance.Amount}");

Console.WriteLine("Withdrawing some money.");
var amount = await account.Withdraw(Random.Shared.NextDouble() * balance.Amount);
Console.WriteLine($"Withdrew: {amount}");