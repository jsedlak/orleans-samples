using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors;

public sealed class AccountSecureSatteliteActor : Grain<OnlineStatus>, IAccountSecureSatelliteActor
{
    public Task<OnlineStatus> GetStatus()
    {
        return Task.FromResult(State);
    }

    private Task<bool> SetStatus(string? status)
    {
        State = new OnlineStatus
        {
            Status = status ?? "",
            IsOnline = status != null
        };

        return Task.FromResult(true);
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        // create the extension, passing it our private method
        var ext = new AccountSecureSatelliteActorExtension(SetStatus);

        // register it!
        GrainContext.SetComponent<IAccountSecureSatelliteActorExtension>(ext);

        return base.OnActivateAsync(cancellationToken);
    }
}