using Orleans.Runtime;
using OrleansSamples.Common.Model;

namespace OrleansSamples.Common.Grains;

public interface IReminderCounterGrain : IGrainWithStringKey
{
    Task<Count> StartCounting();

    Task<Count> StopCounting();

    Task<Count> GetCount();
}
