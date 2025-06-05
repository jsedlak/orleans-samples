using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;
using Orleans.Concurrency;

namespace CachedSiloReads.SiloHost.Grains;

[StatelessWorker(1)]
public class CachingWeatherGrain : Grain, ICachingWeatherGrain
{
    private readonly Dictionary<string, WeatherCacheItem> _cache = new();
    private readonly ILogger<ICachingWeatherGrain> _logger;

    public CachingWeatherGrain(ILogger<ICachingWeatherGrain> logger)
    {
        _logger = logger;
    }

    public async Task<WeatherForecast> GetForecast(string region)
    {

        if (_cache.TryGetValue(region, out var cacheItem) && 
            cacheItem.LastUpdated > DateTime.UtcNow.AddMinutes(-5))
        {
            _logger.LogInformation("Cache hit for region: {Region}", region);

            return cacheItem.Forecast;
        }

        _logger.LogInformation("Cache miss for region: {Region}. Fetching from weather service.", region);

        // Simulate fetching from a weather service
        var weatherGrain = GrainFactory.GetGrain<IWeatherGrain>(region);
        var forecast = await weatherGrain.GetForecast();

        _cache[region] = new WeatherCacheItem
        {
            Forecast = forecast,
            LastUpdated = DateTime.UtcNow
        };

        return forecast;
    }

    private class WeatherCacheItem
    {
        public WeatherForecast Forecast { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
