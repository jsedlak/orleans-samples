using Microsoft.AspNetCore.Mvc;
using OrleansSamples.Common.Grains;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.AddKeyedAzureTableClient("reminders");
builder.UseOrleans();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/api/counters/{id}/start", async (
    [FromRoute] string id,
    [FromServices] IClusterClient clusterClient) =>
{
    var grain = clusterClient.GetGrain<IReminderCounterGrain>(id);
    return await grain.StartCounting();
});

app.MapGet("/api/counters/{id}/stop", async (
    [FromRoute] string id,
    [FromServices] IClusterClient clusterClient) =>
{
    var grain = clusterClient.GetGrain<IReminderCounterGrain>(id);
    return await grain.StopCounting();
});

app.MapGet("/api/counters/{id}", async (
    [FromRoute] string id,
    [FromServices] IClusterClient clusterClient) =>
{
    var grain = clusterClient.GetGrain<IReminderCounterGrain>(id);
    return await grain.GetCount();
});

await app.RunAsync();

