using OrleansSamples.Patterns.SatellitePattern.Domain.Aggregates;

namespace OrleansSamples.Patterns.SatellitePattern.Grains.GrainExtensions;

public interface IExtensionAccountActor : IAccount, IGrainWithGuidKey
{
}
