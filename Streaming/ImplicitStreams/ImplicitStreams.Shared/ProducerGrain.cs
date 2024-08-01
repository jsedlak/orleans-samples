using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Streams;

namespace ImplicitStreams.Shared;

public sealed class ProducerGrain : Grain, IProducerGrain
{
    private readonly ILogger<IProducerGrain> _logger;
    
    private IAsyncStream<int>? _stream;
    private IDisposable? _timer;

    private int _counter = 0;

    public ProducerGrain(ILogger<IProducerGrain> logger)
    {
        _logger = logger;
    }
    
    public Task StartProducing()
    {
        // Get the stream
        var streamId = StreamId.Create(Constants.StreamNamespace, this.GetGrainId().GetGuidKey());
        _stream = this
            .GetStreamProvider(Constants.StreamProvider)
            .GetStream<int>(streamId);

        // Register a timer that produce an event every second
        var period = TimeSpan.FromSeconds(1);
        _timer = this.RegisterGrainTimer<object>(TimerTick, new { }, period, period);

        _logger.LogInformation("I will produce a new event every {Period}", period);

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
    
    private async Task TimerTick(object _)
    {
        var value = _counter++;
        
        _logger.LogInformation("Sending event {EventNumber}", value);
        
        if (_stream is not null)
        {
            await _stream.OnNextAsync(value);
        }
    }
}