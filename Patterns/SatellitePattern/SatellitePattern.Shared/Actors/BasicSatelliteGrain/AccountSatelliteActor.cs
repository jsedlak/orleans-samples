using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors.BasicSatelliteGrain;

public sealed class AccountSatelliteActor : Grain<OnlineStatus>, IAccountSatelliteActor
{
    public Task<OnlineStatus> GetStatus()
    {
        return Task.FromResult(State);
    }

    public Task<bool> SetStatus(string? status)
    {
        State = new OnlineStatus
        {
            Status = status ?? "",
            IsOnline = status != null
        };

        return Task.FromResult(true);
    }
}