using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Grains.GrainExtensions;

namespace OrleansSamples.Patterns.SatellitePattern.Silo.Grains.GrainExtensions;

public class ExtendedAccountStatusActor : Grain<OnlineStatus>, IExtendedAccountStatusActor
{
    private async Task<bool> SetStatus(string? status)
    {
        State = new OnlineStatus
        {
            AccountId = this.GetGrainId().GetGuidKey(),
            Status = status ?? "Offline",
            IsOnline = status != null
        };

        await WriteStateAsync();

        return true;
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        // create the extension, passing it our private method
        var ext = new AccountSatelliteActorExtension(SetStatus);

        // register it!
        GrainContext.SetComponent<IAccountSatelliteActorExtension>(ext);

        return base.OnActivateAsync(cancellationToken);
    }

    public Task<OnlineStatus> GetStatus()
    {
        return Task.FromResult(State);
    }
}