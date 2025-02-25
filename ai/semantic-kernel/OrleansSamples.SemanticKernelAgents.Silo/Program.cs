using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using OrleansSamples.SemanticKernelAgents.GrainModel;
using OrleansSamples.SemanticKernelAgents.Model;
using OrleansSamples.SemanticKernelAgents.Plugins;
using OrleansSamples.SemanticKernelAgents.Silo;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

/* ############################################ */
/* CONFIGURE SEMANTIC KERNEL                    */
/* ############################################ */

/* 
 * Uncomment to use Ollama. Function calling should work
 * with llama3.1 or later, but this has not been tested.
 */
//#pragma warning disable SKEXP0070
//builder.Services.AddOllamaChatCompletion(
//    modelId: "llama3.1",
//    endpoint: new Uri("http://localhost:11434")
//);

/*
 * Configure OpenAI for chat completion. Function calling
 * will work with gpt-4o-mini, as well as others.
 */
builder.Services.AddOpenAIChatCompletion(
    modelId: "gpt-4o-mini",
    apiKey: builder.Configuration["OpenAI:ApiKey"] ?? "NO_KEY"
);

/*
 * Uncomment to use Azure Open AI for chat completion.
 * This has been tested to work using gpt-4o-mini.
 */
//builder.Services.AddAzureOpenAIChatCompletion(
//    deploymentName: "DEPLOYMENT_NAME",
//    endpoint: "https://ENDPOINT_NAME.openai.azure.com/",
//    apiKey: builder.Configuration["OpenAI:ApiKey"] ?? "NO_KEY",
//    modelId: "gpt-4o-mini"
//);

builder.Services.AddTransient((serviceProvider) =>
{
    var kernel = new Kernel(serviceProvider);
    kernel.Plugins.AddFromObject(new LightsPlugin([
        new LightModel { Id = 1, Name = "Table Lamp", IsOn = false, Brightness = Brightness.Medium, Color = "#FFFFFF" },
        new LightModel { Id = 2, Name = "Porch light", IsOn = false, Brightness = Brightness.High, Color = "#FF0000" },
        new LightModel { Id = 3, Name = "Chandelier", IsOn = true, Brightness = Brightness.Low, Color = "#FFFF00" }
    ]));
    
    return kernel;
});

/* ############################################ */
/* CONFIGURE ORLEANS (Yes, it's that simple)    */
/* ############################################ */
builder.UseOrleans();

// Build the app
var app = builder.Build();

app.MapDefaultEndpoints();
app.UseHttpsRedirection();

// Provide an API into the chat agent
app.MapGrainEndpoints();

// Run the app!
await app.RunAsync();