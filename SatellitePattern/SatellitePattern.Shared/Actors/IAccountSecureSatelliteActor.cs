using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors;

/// <summary>
/// Represents a more secure approach to the satellite pattern using a grain and a grain extension
/// </summary>
public interface IAccountSecureSatelliteActor : IGrainWithGuidKey
{
    Task<OnlineStatus> GetStatus();
}
