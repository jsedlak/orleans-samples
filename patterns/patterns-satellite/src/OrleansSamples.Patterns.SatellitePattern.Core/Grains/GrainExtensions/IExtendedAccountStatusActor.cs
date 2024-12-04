using OrleansSamples.Patterns.SatellitePattern.Domain.Model;

namespace OrleansSamples.Patterns.SatellitePattern.Grains.GrainExtensions;

public interface IExtendedAccountStatusActor : IGrainWithGuidKey
{
    Task<OnlineStatus> GetStatus();
}