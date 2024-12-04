using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Grains.BasicSatelliteGrain;

namespace OrleansSamples.Patterns.SatellitePattern.Silo.Grains.BasicSatelliteGrain;

public class BasicAccountStatusActor : Grain<OnlineStatus>, IAccountStatusActor
{
    public Task<OnlineStatus> GetStatus()
    {
        return Task.FromResult(State);
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        State.AccountId = this.GetGrainId().GetGuidKey();
        return base.OnActivateAsync(cancellationToken);
    }

    public async Task<bool> SetStatus(string? status)
    {
        if(status is null)
        {
            State.Status = "Offline";
            State.IsOnline = false;
        }
        else
        {
            State.Status = status;
            State.IsOnline = true;
        }

        State.LastSeen = DateTime.UtcNow;

        await WriteStateAsync();

        return true;
    }
}