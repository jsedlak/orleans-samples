using CachedSiloReads.SiloHost.Model;

namespace CachedSiloReads.SiloHost.GrainModel;

public interface IPushingWeatherGrain : IGrainWithStringKey
{
    IAsyncEnumerable<WeatherForecast> GetForecastUpdates();
}
