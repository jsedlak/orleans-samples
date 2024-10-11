using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors;

/// <summary>
/// Represents a standard approach to the satellite pattern using a grain
/// </summary>
public interface IAccountSatelliteActor : IGrainWithGuidKey
{
    Task<bool> SetStatus(string? status);

    Task<OnlineStatus> GetStatus();
}
