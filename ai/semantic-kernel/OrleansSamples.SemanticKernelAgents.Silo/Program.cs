using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using OrleansSamples.SemanticKernelAgents.GrainModel;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// add our semantic kernel
// change to whatever model you want
#pragma warning disable SKEXP0070
builder.Services.AddOllamaChatCompletion(
    modelId: "smallthinker", 
    endpoint: new Uri("http://localhost:11434")
);

builder.Services.AddTransient((serviceProvider) => {
    return new Kernel(serviceProvider);
});

builder.UseOrleans();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseHttpsRedirection();

app.MapPost("api/agents/{agentId}/chat", 
    async ([FromServices] IClusterClient cluster, [FromRoute] string agentId, [FromBody]UserChatRequest request) =>
    {
        var account = cluster.GetGrain<IChatAgentGrain>(agentId);
        var response = await account.Submit(request.message);
        return new { message = response };
    }
)
.WithName("Agent_SubmitChat");

await app.RunAsync();

public record UserChatRequest(string message);