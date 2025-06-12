using CachedSiloReads.SiloHost.Model;
using Orleans.Concurrency;

namespace CachedSiloReads.SiloHost.GrainModel;

public interface IReceivingCachingWeatherGrain : IGrainWithStringKey
{
    [ReadOnly]
    Task<WeatherForecast> GetForecast();

    Task Deactivate();
}
