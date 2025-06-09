using CachedSiloReads.SiloHost.Model;

namespace CachedSiloReads.SiloHost.GrainModel;

public interface IReceivingCachingWeatherGrain : IGrainWithStringKey
{
    Task<WeatherForecast?> GetForecast();

    Task Deactivate();
}
