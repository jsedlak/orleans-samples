using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Grains;
using CachedSiloReads.SiloHost.Model;
using ZiggyCreatures.Caching.Fusion;

namespace CachedSiloReads.SiloHost.Services;

public class CachedWeatherService
{
    private readonly IClusterClient _clusterClient;
    private readonly IFusionCache _fusionCache;

    public CachedWeatherService(IClusterClient clusterClient, IFusionCache fusionCache)
    {
        _clusterClient = clusterClient;
        _fusionCache = fusionCache;
    }

    public async Task<WeatherForecast> GetForecastForRegion(string region)
    {
        return await _fusionCache.GetOrSetAsync<WeatherForecast>(
            AutoCachingWeatherGrain.FormatCacheKey(region),
            async (t) =>
            {
                Console.WriteLine("Cache miss, generating new forecast for region: {0}", region);

                var grain = _clusterClient.GetGrain<IAutoCachingWeatherGrain>(region);
                return await grain.GetForecast();
            }
        );
    }
}