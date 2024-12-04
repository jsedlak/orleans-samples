using OrleansSamples.Patterns.SatellitePattern.Domain.Model;

namespace OrleansSamples.Patterns.SatellitePattern.Domain.ServiceModel;

public interface IAccountStatusWriteRepository
{
    Task Upsert(OnlineStatus status);
}