namespace CachedSiloReads.SiloHost.Grains;

using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;

public class WeatherMonitorGrain(ILogger<WeatherMonitorGrain> logger) : MonitorGrain<WeatherForecast>(logger)
{
    protected override IAsyncEnumerable<WeatherForecast> GetUpdates()
    {
        return GrainFactory
            .GetGrain<IPushingWeatherGrain>("the-weather-grain")
            .GetForecastUpdates();
    }
}
