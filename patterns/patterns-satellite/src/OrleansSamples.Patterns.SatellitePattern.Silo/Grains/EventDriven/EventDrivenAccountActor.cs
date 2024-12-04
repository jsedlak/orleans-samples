﻿using Orleans.Streams;
using OrleansSamples.Patterns.SatellitePattern.Domain.Events;
using OrleansSamples.Patterns.SatellitePattern.Grains.EventDriven;

namespace OrleansSamples.Patterns.SatellitePattern.Silo.Grains.EventDriven;

public class EventDrivenAccountActor : Grain, IEventDrivenAccountActor
{
    private IAsyncStream<OnlineStatusSetEvent>? _stream;

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        // Get the stream
        var streamId = StreamId.Create(EventDrivenConstants.StreamNamespace, this.GetGrainId().GetGuidKey());
        _stream = this
            .GetStreamProvider(EventDrivenConstants.StreamProvider)
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

    public async Task<bool> SignIn(string status)
    {
        if(_stream is null)
        {
            return false;
        }

        await _stream.OnNextAsync(new OnlineStatusSetEvent
        {
            AccountId = this.GetGrainId().GetGuidKey(),
            Status = status
        });

        return true;
    }

    public async Task<bool> SignOut()
    {
        if (_stream is null)
        {
            return false;
        }

        await _stream.OnNextAsync(new OnlineStatusSetEvent
        {
            AccountId = this.GetGrainId().GetGuidKey(),
            Status = null
        });

        return true;
    }
}
