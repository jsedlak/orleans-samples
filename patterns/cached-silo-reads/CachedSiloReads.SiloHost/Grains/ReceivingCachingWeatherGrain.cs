using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;

namespace CachedSiloReads.SiloHost.Grains;

public class ReceivingCachingWeatherGrain(ILogger<ICachingWeatherGrain> logger) : 
    ReceivingCachingGrain<WeatherForecast>(logger)
{
    protected override IAsyncEnumerable<WeatherForecast> GetUpdates()
    {
        return GrainFactory
            .GetGrain<IPushingWeatherGrain>("the-weather-grain")
            .GetForecastUpdates();
    }
}
