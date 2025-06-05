using CachedSiloReads.SiloHost.Model;

namespace CachedSiloReads.SiloHost.GrainModel;

public interface IWeatherGrain : IGrainWithStringKey
{
    Task<WeatherForecast> GetForecast();
}
