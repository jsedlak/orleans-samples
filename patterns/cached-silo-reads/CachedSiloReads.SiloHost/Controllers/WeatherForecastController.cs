using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;

namespace CachedSiloReads.SiloHost.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IClusterClient _clusterClient;

    public WeatherForecastController(IClusterClient clusterClient, ILogger<WeatherForecastController> logger)
    {
        _clusterClient = clusterClient;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get([FromQuery]string region)
    {
        var result = await _clusterClient
            .GetGrain<ICachingWeatherGrain>(0)
            .GetForecast(region);

        return [result];
    }
}
