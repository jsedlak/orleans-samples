using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Orleans.Providers.MongoDB.Utils;
using OrleansSamples.Common.Grains;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.AddKeyedMongoDBClient("clustering");

// We have to provide a singleton of IMongoClient and IMongoClientFactory
// for the MongoDBMembershipTableOptions class, but these are not registered
// automatically by the AddKeyedMongoDBClient extension
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new MongoClient(
        config.GetConnectionString("clustering")
    );
});

builder.Services.AddSingleton<IMongoClientFactory, DefaultMongoClientFactory>(sp => 
    new DefaultMongoClientFactory(sp.GetRequiredService<IMongoClient>())
);

// Register Orleans
builder.UseOrleans();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure endpoints to grant access to the cluster
app.MapGet("/account/{accountId}", async ([FromServices] IClusterClient cluster, [FromRoute] int accountId) =>
{
    var account = cluster.GetGrain<IAccountGrain>(accountId);
    return await account.GetBalance();
})
.WithName("GetAccount");

app.MapPost("/account/{accountId}/deposit",
    async ([FromServices] IClusterClient cluster, [FromRoute] int accountId, [FromBody] AmountRequest request) =>
    {
        var account = cluster.GetGrain<IAccountGrain>(accountId);
        var balance = await account.Deposit(request.amount);
        return new { balance };
    })
.WithName("DepositToAccount");

app.MapPost("/account/{accountId}/withdraw",
    async ([FromServices] IClusterClient cluster, [FromRoute] int accountId, [FromBody] AmountRequest request) =>
    {
        var account = cluster.GetGrain<IAccountGrain>(accountId);
        var amountWithdrawn = await account.Withdraw(request.amount);
        return new { amountWithdrawn };
    })
.WithName("WithdrawFromAccount");

app.UseHttpsRedirection();

await app.RunAsync();

record AmountRequest(double amount);
