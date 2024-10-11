namespace SatellitePattern.Shared.Actors;

public interface IAccountSecureSatelliteActorExtension : IGrainExtension
{
    Task<bool> SetStatus(string? status);
}
