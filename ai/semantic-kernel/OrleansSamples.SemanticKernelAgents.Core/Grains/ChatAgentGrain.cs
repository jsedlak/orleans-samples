using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OrleansSamples.SemanticKernelAgents.GrainModel;
using OrleansSamples.SemanticKernelAgents.Model;

namespace OrleansSamples.SemanticKernelAgents.Grains;

public class ChatAgentGrain : Grain, IChatAgentGrain
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly IPersistentState<AgentState> _agentData;
    private readonly PromptExecutionSettings _executionSettings = new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };

    public ChatAgentGrain(
        [PersistentState("history")]IPersistentState<AgentState> agentData, 
        Kernel kernel)
    {
        _agentData = agentData;
        _kernel = kernel;   
        _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
    }

    public async Task<string> Submit(string userMessage)
    {
        // Take the last N messages from our history,
        // add our new message and save it to the state
        _agentData.State.Messages = [
            .._agentData.State.Messages.TakeLast(_agentData.State.MaxChatHistoryLength - 1), 
            new() {
            Type = ChatMessageType.User,
            Content = userMessage
            }
        ];

        await _agentData.WriteStateAsync();

        // Build the history for the kernel
        ChatHistory history = new ChatHistory();
        
        // add our developer prompt
        if (!string.IsNullOrWhiteSpace(_agentData.State.DeveloperPrompt))
        {
            history.AddDeveloperMessage(_agentData.State.DeveloperPrompt);
        }

        // add our history, this will include our new message
        foreach(var msg in _agentData.State.Messages)
        {
            history.AddMessage(
                msg.Type == ChatMessageType.System ? AuthorRole.System : AuthorRole.User,
                msg.Content
            );
        }

        // do AI things and get the response
        var response = await _chatCompletionService.GetChatMessageContentsAsync(
            history,
            kernel: _kernel,
            executionSettings: _executionSettings
        );

        var agentResponseContent = response[^1]?.Content ?? "Error - could not retrieve a response.";

        // write to the history
        _agentData.State.Messages = [
            .._agentData.State.Messages,
            new () { Content = agentResponseContent, Type = ChatMessageType.System }
        ];

        await _agentData.WriteStateAsync();

        return agentResponseContent;
    }

    public Task<ChatMessage[]> GetHistory()
    {
        return Task.FromResult(
            _agentData.State.Messages
        );
    }

    public async Task SetDeveloperPrompt(string message)
    {
        _agentData.State.DeveloperPrompt = message;
        await _agentData.WriteStateAsync();
    }

    public Task<string> GetDeveloperPrompt()
    {
        return Task.FromResult(
            _agentData.State.DeveloperPrompt
        );
    }

    public async Task SetHistoryMaxLength(int amount)
    {
        _agentData.State.MaxChatHistoryLength = amount;
        await _agentData.WriteStateAsync();
    }

    public Task<int> GetHistoryMaxLength()
    {
        return Task.FromResult(
            _agentData.State.MaxChatHistoryLength
        );
    }
}
