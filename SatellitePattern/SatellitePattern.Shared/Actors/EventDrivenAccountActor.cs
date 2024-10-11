using Orleans.Streams;
using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors;

public sealed class EventDrivenAccountActor : Grain<AccountState>, IEventDrivenAccountActor
{
    private IAsyncStream<OnlineStatusSetEvent>? _stream;

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        // Get the stream
        var streamId = StreamId.Create(Constants.StreamNamespace, this.GetGrainId().GetGuidKey());
        _stream = this
            .GetStreamProvider(Constants.StreamProvider)
            .GetStream<OnlineStatusSetEvent>(streamId);

        return base.OnActivateAsync(cancellationToken);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        if (_stream is not null)
        {
            _stream = null;
        }

        return base.OnDeactivateAsync(reason, cancellationToken);
    }

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
        await _stream.OnNextAsync(new OnlineStatusSetEvent
        {
            AccountId = this.GetGrainId().GetGuidKey(),
            Status = status
        });

        return true;
    }

    public async ValueTask<bool> SignOut()
    {
        await _stream.OnNextAsync(new OnlineStatusSetEvent
        {
            AccountId = this.GetGrainId().GetGuidKey(),
            Status = null
        });

        return true;
    }
}
