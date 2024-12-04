using OrleansSamples.Patterns.SatellitePattern.Domain.Aggregates;

namespace OrleansSamples.Patterns.SatellitePattern.Grains.BasicSatelliteGrain;

/// <summary>
/// Provides an interface into the basic account actor implementation
/// </summary>
public interface IBasicAccountActor : IAccount, IGrainWithGuidKey
{
}
