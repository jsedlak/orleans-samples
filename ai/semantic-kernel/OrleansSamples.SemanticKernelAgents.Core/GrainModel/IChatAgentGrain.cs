namespace OrleansSamples.SemanticKernelAgents.GrainModel;

public interface IChatAgentGrain : IGrainWithStringKey
{
    public Task<string> Submit(string userMessage);
}
