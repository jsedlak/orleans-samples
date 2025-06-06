using CachedSiloReads.SiloHost.Model;

namespace CachedSiloReads.SiloHost.GrainModel;

public interface IAutoCachingWeatherGrain : IGrainWithStringKey
{
    Task<WeatherForecast> GetForecast();
}