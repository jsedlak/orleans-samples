using Microsoft.AspNetCore.Mvc;
using OrleansSamples.Common.Grains;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.UseOrleans();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapGet("/api/producers/{id}/start", async (
    [FromRoute] string id,
    [FromServices] IClusterClient clusterClient) =>
{
    var grain = clusterClient.GetGrain<IStreamProducerGrain>(id);
    await grain.StartProducing();

    return new { success = true };
});

app.MapGet("/api/consumers/{id}", async (
    [FromRoute] string id,
    [FromServices] IClusterClient clusterClient) =>
{
    var grain = clusterClient.GetGrain<IStreamConsumerGrain>(id);
    return await grain.GetCount();
});

await app.RunAsync();