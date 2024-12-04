using OrleansSamples.Patterns.SatellitePattern.Grains.GrainExtensions;

namespace OrleansSamples.Patterns.SatellitePattern.Silo.Grains.GrainExtensions;

public sealed class AccountSatelliteActorExtension : IAccountSatelliteActorExtension
{
    private readonly Func<string?, Task<bool>> _statusHandler;

    public AccountSatelliteActorExtension(Func<string?, Task<bool>> statusHandler)
    {
        _statusHandler = statusHandler;
    }

    public async Task<bool> SetStatus(string? status)
    {
        var result = await _statusHandler(status);
        return result;
    }
}
