using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors;

public sealed class SecureAccountActor : Grain<AccountState>, ISecureAccountActor
{
    public Task<DirectMessage[]> GetMessages()
    {
        return Task.FromResult(State.Messages);
    }

    public async Task SendMessage(Guid senderAccountId, string message)
    {
        State.Messages = State.Messages.Append(new DirectMessage
        {
            SenderAccountId = senderAccountId,
            Message = message
        }).ToArray();

        await WriteStateAsync();
    }

    public async ValueTask<bool> SignIn(string status)
    {
        var satelliteGrain = GrainFactory.GetGrain<IAccountSecureSatelliteActor>(this.GetGrainId().GetGuidKey());
        var ext = satelliteGrain.AsReference<IAccountSecureSatelliteActorExtension>();

        var result = await ext.SetStatus(status);

        return result;
    }

    public async ValueTask<bool> SignOut()
    {
        var satelliteGrain = GrainFactory.GetGrain<IAccountSecureSatelliteActor>(this.GetGrainId().GetGuidKey());
        var ext = satelliteGrain.AsReference<IAccountSecureSatelliteActorExtension>();

        var result = await ext.SetStatus(null);

        return result;
    }
}
