using Microsoft.AspNetCore.Mvc;
using OrleansSamples.Common.Grains;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.AddKeyedMongoDBClient("clustering");
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
