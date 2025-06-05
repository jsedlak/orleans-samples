using CachedSiloReads.SiloHost.Model;

namespace CachedSiloReads.SiloHost.GrainModel;

public interface ICachingWeatherGrain : IGrainWithIntegerKey
{
    Task<WeatherForecast> GetForecast(string region);
}