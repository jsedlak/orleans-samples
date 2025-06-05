using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;
using CachedSiloReads.SiloHost.Services;

namespace CachedSiloReads.SiloHost.Grains;

public class WeatherGrain : Grain, IWeatherGrain
{
    private readonly WeatherService _weatherService;
    private readonly IPersistentState<WeatherForecast> _state;

    public WeatherGrain(
        WeatherService weatherService,
        [PersistentState("weather")] IPersistentState<WeatherForecast> state)
    {
        _weatherService = weatherService;
        _state = state;
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        this.RegisterGrainTimer(
            this.UpdateWeather, 
            (object?)null, 
            TimeSpan.FromSeconds(60),
            TimeSpan.FromSeconds(60)
        );

        return base.OnActivateAsync(cancellationToken);
    }

    public Task<WeatherForecast> GetForecast()
    {
        return Task.FromResult(_state.State);
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
    }
}
