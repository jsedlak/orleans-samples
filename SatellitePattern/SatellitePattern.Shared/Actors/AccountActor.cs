using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors;

public sealed class AccountActor : Grain<AccountState>, IAccountActor
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
        await GrainFactory
            .GetGrain<IAccountSatelliteActor>(this.GetGrainId().GetGuidKey())
            .SetStatus(status);

        return true;
    }

    public async ValueTask<bool> SignOut()
    {
        await GrainFactory
            .GetGrain<IAccountSatelliteActor>(this.GetGrainId().GetGuidKey())
            .SetStatus(null);

        return true;
    }
}
