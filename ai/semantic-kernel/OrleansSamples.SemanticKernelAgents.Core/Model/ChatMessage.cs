namespace OrleansSamples.SemanticKernelAgents.Model;

[GenerateSerializer]
public class ChatMessage
{
    [Id(0)]
    public string Content { get; set; } = null!;

    [Id(1)]
    public ChatMessageType Type { get; set; } = ChatMessageType.System;
}
