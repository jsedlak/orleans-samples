namespace OrleansSamples.Common.Grains;

/// <summary>
/// Provides a simple counting mechanism
/// </summary>
public interface ICountGrain : IGrainWithIntegerKey
{
    /// <summary>
    /// Increments the count and returns the new value
    /// </summary>
    /// <returns></returns>
    ValueTask<int> Increment();
}