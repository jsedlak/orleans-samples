using OrleansSamples.Patterns.SatellitePattern.Domain.Model;

namespace OrleansSamples.Patterns.SatellitePattern.Grains.BasicSatelliteGrain;

public interface IAccountStatusActor : IGrainWithGuidKey
{
    Task<bool> SetStatus(string? status);

    Task<OnlineStatus> GetStatus();
}