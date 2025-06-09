using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;
using CachedSiloReads.SiloHost.Services;
using System.Threading.Channels;

namespace CachedSiloReads.SiloHost.Grains;

public class PushingWeatherGrain : Grain, IPushingWeatherGrain
{
    private readonly WeatherService _weatherService;
    private readonly IPersistentState<WeatherForecast> _state;

    private readonly HashSet<Channel<WeatherForecast>> _subscribers = [];

    public PushingWeatherGrain(
        WeatherService weatherService,
        [PersistentState("pushingweather")] IPersistentState<WeatherForecast> state)
    {
        _weatherService = weatherService;
        _state = state;
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        this.RegisterGrainTimer(
            this.UpdateWeather,
            (object?)null,
            new GrainTimerCreationOptions(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
            {
                Interleave = false
            }
        );

        return base.OnActivateAsync(cancellationToken);
    }

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        foreach (var channel in _subscribers)
        {
            channel.Writer.TryComplete();
        }

        return base.OnDeactivateAsync(reason, cancellationToken);
    }


    private async Task UpdateWeather(object? state)
    {
        _state.State = _weatherService.Get().FirstOrDefault() ?? new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = 0,
            Summary = "No data available"
        };

        await _state.WriteStateAsync();

        // Push the update to all subscribers via their channels.
        await Task.WhenAll(
            _subscribers.Select(
                channel => channel.Writer.WriteAsync(_state.State).AsTask()));
    }

    public async IAsyncEnumerable<WeatherForecast> GetForecastUpdates()
    {
        // Add caller as a new subscriber.
        var channel = Channel.CreateBounded<WeatherForecast>(
            new BoundedChannelOptions(capacity: 1)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = true
            });

        _subscribers.Add(channel);

        try
        {
            // provide the latest forecast as an initial update to the new subscriber.
            await channel.Writer.WriteAsync(_state.State);

            await foreach (var forecast in channel.Reader.ReadAllAsync())
            {
                yield return forecast;
            }
        }
        finally
        {
            _subscribers.Remove(channel);
        }
    }
}
