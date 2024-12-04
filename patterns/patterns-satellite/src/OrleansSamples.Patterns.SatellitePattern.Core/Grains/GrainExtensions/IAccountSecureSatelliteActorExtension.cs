namespace OrleansSamples.Patterns.SatellitePattern.Grains.GrainExtensions;

public interface IAccountSatelliteActorExtension : IGrainExtension
{
    Task<bool> SetStatus(string? status);
}
