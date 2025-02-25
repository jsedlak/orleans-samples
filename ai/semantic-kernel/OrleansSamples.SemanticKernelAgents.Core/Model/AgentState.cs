namespace OrleansSamples.SemanticKernelAgents.Model;

[GenerateSerializer]
public class AgentState
{
    [Id(0)]
    public string DeveloperPrompt { get; set; } = "";

    [Id(1)]
    public ChatMessage[] Messages { get; set; } = Array.Empty<ChatMessage>();

    [Id(2)]
    public int MaxChatHistoryLength { get; set; } = 5;

    
}