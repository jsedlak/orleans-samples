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

app.Run();

record AmountRequest(double amount);