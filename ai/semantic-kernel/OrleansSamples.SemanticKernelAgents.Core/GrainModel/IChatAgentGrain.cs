using OrleansSamples.SemanticKernelAgents.Model;

namespace OrleansSamples.SemanticKernelAgents.GrainModel;

public interface IChatAgentGrain : IGrainWithStringKey
{
    public Task<string> Submit(string userMessage);

    public Task<ChatMessage[]> GetHistory();

    public Task SetDeveloperPrompt(string message);

    public Task<string> GetDeveloperPrompt();

    public Task SetHistoryMaxLength(int amount);

    public Task<int> GetHistoryMaxLength();
}
