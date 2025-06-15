using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;
using CachedSiloReads.SiloHost.Services;
using Microsoft.AspNetCore.Mvc;

namespace CachedSiloReads.SiloHost.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IClusterClient _clusterClient;
    private readonly CachedWeatherService _cachedWeatherService;

    public WeatherForecastController(IClusterClient clusterClient, ILogger<WeatherForecastController> logger, CachedWeatherService cachedWeatherService)
    {
        _clusterClient = clusterClient;
        _logger = logger;
        _cachedWeatherService = cachedWeatherService;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get([FromQuery]string region)
    {
        var result = await _clusterClient
            .GetGrain<ICachingWeatherGrain>(0)
            .GetForecast(region);

        return [result];
    }

    [HttpGet("autocached")]
    public async Task<IEnumerable<WeatherForecast>> GetWithCaching([FromQuery]string region)
    {
        var result = await _cachedWeatherService
            .GetForecastForRegion(region);

        return [result];
    }

    [HttpGet("pushcaching")]
    public async Task<WeatherForecast?> GetWithPushCaching([FromQuery]string cache)
    {
        return await _clusterClient
            .GetGrain<IReceivingCachingGrain<WeatherForecast>>(cache)
            .GetCurrentValue();
    }

    [HttpGet("stoppushcaching")]
    public async Task StopPushCaching([FromQuery]string cache)
    {
        await _clusterClient
            .GetGrain<IReceivingCachingGrain<WeatherForecast>>(cache)
            .Abort();
    }
}
