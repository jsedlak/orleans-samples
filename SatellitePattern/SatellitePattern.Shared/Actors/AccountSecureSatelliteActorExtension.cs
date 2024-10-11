namespace SatellitePattern.Shared.Actors;

public sealed class AccountSecureSatelliteActorExtension : IAccountSecureSatelliteActorExtension
{
    private readonly Func<string?, Task<bool>> _statusHandler;

    public AccountSecureSatelliteActorExtension(Func<string?, Task<bool>> statusHandler)
    {
        _statusHandler = statusHandler;
    }

    public async Task<bool> SetStatus(string? status)
    {
        var result = await _statusHandler(status);
        return result;
    }
}