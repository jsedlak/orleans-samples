using OrleansSamples.Patterns.SatellitePattern.Domain.Aggregates;

namespace OrleansSamples.Patterns.SatellitePattern.Grains.EventDrivenDatabase;

public interface IEventDrivenDatabaseAccountActor : IAccount, IGrainWithGuidKey
{
}
