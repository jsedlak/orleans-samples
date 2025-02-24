using OrleansSamples.Common;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Register custom services
builder.Services.AddOpenApi();

// Register our aspire services
builder.AddKeyedRedisClient("redis");

// Add Orleans
builder.UseOrleans();

var app = builder.Build();

// add open api
app.MapOpenApi();

// Map all endpoints
app.MapDefaultEndpoints();
app.MapGrainEndpoints();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

await app.RunAsync();