using Microsoft.AspNetCore.Mvc;
using OrleansSamples.Common.Grains;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyedAzureBlobClient("GrainStorage");

// Add services to the container.
builder.AddServiceDefaults();
builder.UseOrleans();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Configure endpoints to grant access to the cluster
app.MapGet("/account/{accountId}", async ([FromServices] IClusterClient cluster, [FromRoute]int accountId) =>
{
    var account = cluster.GetGrain<IAccountGrain>(accountId);
    return await account.GetBalance();
})
.WithName("GetAccount");

app.MapPost("/account/{accountId}/deposit", 
    async ([FromServices] IClusterClient cluster, [FromRoute] int accountId, [FromBody]AmountRequest request) =>
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

// Add count endpoints
app.MapGet("/count/{counterId}", async ([FromServices] IClusterClient cluster, [FromRoute] int counterId) =>
{
    var counter = cluster.GetGrain<ICountGrain>(counterId);
    var amount = await counter.Increment();
    return new { amount };
}).WithName("IncrementCounter");

app.Run();

record AmountRequest(double amount);