namespace CachedSiloReads.SiloHost.Grains;

using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;

public class WeatherMonitorGrain(ILogger<WeatherMonitorGrain> logger) : MonitorGrain<WeatherForecast>(logger)
{
    protected override IAsyncEnumerable<WeatherForecast> GetUpdates()
    {
        var weatherGrain = GrainFactory.GetGrain<IPushingWeatherGrain>("the-one-and-only");
        logger.LogInformation("{MonitorGrainId} is monitoring {WeatherGrainId}.", this.GetGrainId(), weatherGrain.GetGrainId());

        return weatherGrain.GetForecastUpdates();
    }
}
