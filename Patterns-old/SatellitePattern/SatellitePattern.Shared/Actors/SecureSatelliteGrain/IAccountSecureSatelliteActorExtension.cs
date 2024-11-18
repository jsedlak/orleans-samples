namespace SatellitePattern.Shared.Actors.SecureSatelliteGrain;

public interface IAccountSecureSatelliteActorExtension : IGrainExtension
{
    Task<bool> SetStatus(string? status);
}
