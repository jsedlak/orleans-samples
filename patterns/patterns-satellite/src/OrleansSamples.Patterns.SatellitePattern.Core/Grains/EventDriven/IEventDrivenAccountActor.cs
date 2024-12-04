using OrleansSamples.Patterns.SatellitePattern.Domain.Aggregates;

namespace OrleansSamples.Patterns.SatellitePattern.Grains.EventDriven;

public interface IEventDrivenAccountActor : IAccount, IGrainWithGuidKey
{
}
