
namespace OrleansSamples.Common.Grains;

public class BasicCountGrain : Grain, ICountGrain
{
    private readonly IPersistentState<int> _count;

    public BasicCountGrain([PersistentState("count")]IPersistentState<int> state)
    {
        _count = state;
    }

    public async ValueTask<int> Increment()
    {
        _count.State++;
        await _count.WriteStateAsync();

        return _count.State;
    }
}
