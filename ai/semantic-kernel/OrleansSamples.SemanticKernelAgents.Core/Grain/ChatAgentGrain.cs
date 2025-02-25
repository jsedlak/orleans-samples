#pragma warning disable SKEXP0001
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OrleansSamples.SemanticKernelAgents.GrainModel;

namespace OrleansSamples.SemanticKernelAgents.Grain;

public class ChatAgentGrain : Orleans.Grain, IChatAgentGrain
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly IChatHistoryReducer _reducer;

    public ChatAgentGrain(Kernel kernel)
    {
        _kernel = kernel;
        _reducer = new ChatHistoryTruncationReducer(targetCount: 2);        
        _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
    }

    public async Task<string> Submit(string userMessage)
    {
        ChatHistory history = [];
        history.AddUserMessage(userMessage);

        PromptExecutionSettings settings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var response = await _chatCompletionService.GetChatMessageContentsAsync(
            history,
            kernel: _kernel,
            executionSettings: settings

        );

        return response[^1]?.Content ?? "Could not retrieve chat response.";
    }
}
