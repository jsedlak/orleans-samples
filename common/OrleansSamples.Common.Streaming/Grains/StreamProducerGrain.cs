using Microsoft.Extensions.Logging;
using Orleans.Streams;

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
            this.GetPrimaryKeyString()
        );

        _stream = this
            .GetStreamProvider(StreamingConstants.StreamProvider)
            .GetStream<int>(streamId);

        var period = TimeSpan.FromSeconds(1);
        _timer = this.RegisterGrainTimer<object?>(TimerTick, null, period, period);

        _logger.LogInformation(
            "Stream Producer [{GrainId}] will now produce a count event every {Period}",
            this.GetGrainId().ToString(),
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

    private async Task TimerTick(object? state, CancellationToken token)
    {
        // is silo shutting down
        if (token.IsCancellationRequested)
        {
            return;
        }

        var value = _counter++;

        if (_stream is not null)
        {
            _logger.LogInformation("[{GrainId}] Sending event {Count}", this.GetGrainId(), value);
            await _stream.OnNextAsync(value);
        }
    }
}