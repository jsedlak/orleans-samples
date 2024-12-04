using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Streams;
using OrleansSamples.Common;

namespace OrleansSamples.Common.Grains;

public sealed class StreamProducerGrain : Grain, IStreamProducerGrain
{
    private readonly ILogger<IStreamProducerGrain> _logger;

    private IAsyncStream<int>? _stream;
    private IGrainTimer? _timer;

    private int _counter = 0;

    public StreamProducerGrain(ILogger<IStreamProducerGrain> logger)
    {
        _logger = logger;
    }

    public Task StartProducing() 
    {
        var streamId = StreamId.Create(
            StreamingConstants.StreamNamespace,
            this.GetGrainId().GetGuidKey()
        );

        _stream = this
            .GetStreamProvider(StreamingConstants.StreamProvider)
            .GetStream<int>(streamId);

        var period = TimeSpan.FromSeconds(1);
        _timer = this.RegisterGrainTimer<object?>(TimerTick, null, period, period);

        _logger.LogInformation(
            "Stream Producer [{GrainId}] will now produce a count event every {Period}",
            this.GetGrainId().GetGuidKey(),
            period
        );

        return Task.CompletedTask;
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        if (_timer is not null)
        {
            _timer.Dispose();
            _timer = null;
        }

        if (_stream is not null)
        {
            _stream = null;
        }
        
        return base.OnDeactivateAsync(reason, cancellationToken);
    }
    
<<<<<<< HEAD:common/OrleansSamples.Common.Streaming/Grains/StreamProducerGrain.cs
    private async Task TimerTick(object? _)
=======
    private async Task TimerTick(object? state, CancellationToken token)
>>>>>>> origin/main:Streaming/ImplicitStreams/ImplicitStreams.Shared/ProducerGrain.cs
    {
        // is silo shutting down
        if (!token.IsCancellationRequested)
        {
            var value = _counter++;

            if (_stream is not null)
            {
                _logger.LogInformation("Sending event {EventNumber}", value);
                await _stream.OnNextAsync(value);
            }
        }
    }
}