using Microsoft.AspNetCore.Mvc;
using OrleansSamples.RedisProviders.Silo.RequestModel;
using OrleansSamples.Common.Grains;

namespace OrleansSamples.Common;

public static class ApplicationBuilderExtensions
{
    public static WebApplication MapGrainEndpoints(this WebApplication builder)
    {
        builder.MapAccountGrainEndpoints();
        builder.MapReminderGrainEndpoints();
        builder.MapStreamingGrainEndpoints();

        return builder;
    }

    public static WebApplication MapStreamingGrainEndpoints(this WebApplication builder)
    {
        builder.MapGet("/api/producers/{id}/start", async (
            [FromRoute] string id,
            [FromServices] IClusterClient clusterClient) =>
        {
            var grain = clusterClient.GetGrain<IStreamProducerGrain>(id);
            await grain.StartProducing();

            return new { success = true };
        })
        .WithName("StartProducerCount");

        builder.MapGet("/api/consumers/{id}", async (
            [FromRoute] string id,
            [FromServices] IClusterClient clusterClient) =>
        {
            var grain = clusterClient.GetGrain<IStreamConsumerGrain>(id);
            return await grain.GetCount();
        })
        .WithName("GetConsumerCount");

        return builder;
    }

    public static WebApplication MapReminderGrainEndpoints(this WebApplication builder)
    {
        builder.MapGet("/api/counters/{id}/start", async (
            [FromRoute] string id,
            [FromServices] IClusterClient clusterClient) =>
        {
            var grain = clusterClient.GetGrain<IReminderCounterGrain>(id);
            return await grain.StartCounting();
        })
        .WithName("StartCounter");

        builder.MapGet("/api/counters/{id}/stop", async (
            [FromRoute] string id,
            [FromServices] IClusterClient clusterClient) =>
        {
            var grain = clusterClient.GetGrain<IReminderCounterGrain>(id);
            return await grain.StopCounting();
        })
        .WithName("StopCounter");

        builder.MapGet("/api/counters/{id}", async (
            [FromRoute] string id,
            [FromServices] IClusterClient clusterClient) =>
        {
            var grain = clusterClient.GetGrain<IReminderCounterGrain>(id);
            return await grain.GetCount();
        })
        .WithName("GetCounterValue");

        return builder;
    }

    public static WebApplication MapAccountGrainEndpoints(this WebApplication builder)
    {
        builder.MapGet("/api/accounts/{accountId}", async ([FromServices] IClusterClient cluster, [FromRoute] int accountId) =>
        {
            var account = cluster.GetGrain<IAccountGrain>(accountId);
            return await account.GetBalance();
        })
        .WithName("GetAccount");

        builder.MapPost("/api/accounts/{accountId}/deposit",
            async ([FromServices] IClusterClient cluster, [FromRoute] int accountId, [FromBody] AmountRequest request) =>
            {
                var account = cluster.GetGrain<IAccountGrain>(accountId);
                var balance = await account.Deposit(request.amount);
                return new { balance };
            })
        .WithName("DepositToAccount");

        builder.MapPost("/api/accounts/{accountId}/withdraw",
            async ([FromServices] IClusterClient cluster, [FromRoute] int accountId, [FromBody] AmountRequest request) =>
            {
                var account = cluster.GetGrain<IAccountGrain>(accountId);
                var amountWithdrawn = await account.Withdraw(request.amount);
                return new { amountWithdrawn };
            })
        .WithName("WithdrawFromAccount");

        return builder;

    }
}
