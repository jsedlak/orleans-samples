using Microsoft.AspNetCore.Mvc;
using OrleansSamples.SemanticKernelAgents.GrainModel;
using OrleansSamples.SemanticKernelAgents.Silo.ApiModel;

namespace OrleansSamples.SemanticKernelAgents.Silo;

public static class EndpointExtensions
{
    public static void MapGrainEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapChatEndpoints();
        builder.MapConfigEndpoints();
    }

    public static void MapChatEndpoints(this IEndpointRouteBuilder builder)
    {
        // Submit a chat message and get a response
        builder.MapPost("api/agents/{agentId}/chat",
            async ([FromServices] IClusterClient cluster, [FromRoute] string agentId, [FromBody] UserChatRequest request) =>
            {
                var agent = cluster.GetGrain<IChatAgentGrain>(agentId);
                var response = await agent.Submit(request.message);
                return new { message = response };
            }
        ).WithName("Agent_SubmitChat");

        // Get all chat history for an agent
        builder.MapGet("api/agents/{agentId}/chat",
            async ([FromServices] IClusterClient cluster, [FromRoute] string agentId) =>
            {
                var agent = cluster.GetGrain<IChatAgentGrain>(agentId);
                return await agent.GetHistory();
            }
        ).WithName("Agent_GetChatHistory");
    }

    public static void MapConfigEndpoints(this IEndpointRouteBuilder builder)
    {
        // Set the developer prompt
        builder.MapPost("api/agents/{agentId}/developer-prompt",
            async ([FromServices] IClusterClient cluster, [FromRoute] string agentId, [FromBody] DeveloperPromptRequest request) =>
            {
                var agent = cluster.GetGrain<IChatAgentGrain>(agentId);
                await agent.SetDeveloperPrompt(request.prompt);
                return new { success = true };
            }
        ).WithName("Agent_SetDeveloperPrompt");

        // Get the developer prompt
        builder.MapGet("api/agents/{agentId}/developer-prompt",
            async ([FromServices] IClusterClient cluster, [FromRoute] string agentId) =>
            {
                var agent = cluster.GetGrain<IChatAgentGrain>(agentId);
                var prompt = await agent.GetDeveloperPrompt();
                return new { prompt };
            }
        ).WithName("Agent_GetDeveloperPrompt");

        // Set the history limit
        builder.MapPost("api/agents/{agentId}/history-limit",
            async ([FromServices] IClusterClient cluster, [FromRoute] string agentId, [FromBody] ChatLimitRequest request) =>
            {
                var agent = cluster.GetGrain<IChatAgentGrain>(agentId);
                await agent.SetHistoryMaxLength(request.limit);
                return new { success = true };
            }
        ).WithName("Agent_SetHistoryLimit");

        // Get the history limit
        builder.MapGet("api/agents/{agentId}/history-limit",
            async ([FromServices] IClusterClient cluster, [FromRoute] string agentId) =>
            {
                var agent = cluster.GetGrain<IChatAgentGrain>(agentId);
                var limit = await agent.GetHistoryMaxLength();
                return new { limit };
            }
        ).WithName("Agent_GetHistoryLimit");
    }
}
