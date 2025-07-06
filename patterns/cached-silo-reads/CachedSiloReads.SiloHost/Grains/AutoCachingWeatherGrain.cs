using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;
using CachedSiloReads.SiloHost.Services;
using ZiggyCreatures.Caching.Fusion;

namespace CachedSiloReads.SiloHost.Grains;

public class AutoCachingWeatherGrain : Grain, IAutoCachingWeatherGrain, IRemindable
{
    public static string FormatCacheKey(string region)
    {
        return $"Weather-{region}";
    }

    private readonly WeatherService _weatherService;
    private readonly IPersistentState<WeatherForecast> _state;
    private readonly IFusionCache _fusionCache;

    public AutoCachingWeatherGrain(
        IFusionCache fusionCache,
        WeatherService weatherService,
        [PersistentState("weather")] IPersistentState<WeatherForecast> state)
    {
        _fusionCache = fusionCache;
        _weatherService = weatherService;
        _state = state;
    }

    public Task<WeatherForecast> GetForecast()
    {
        return Task.FromResult(_state.State);
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        // Register the grain timer to update weather data every 30 seconds
        await this.RegisterOrUpdateReminder(
            "UpdateWeatherReminder",
            TimeSpan.FromSeconds(0),
            TimeSpan.FromMinutes(1)
        );

        if (_state.RecordExists)
        {
            return;
        }

        _state.State = _weatherService.Get().FirstOrDefault() ?? new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = 0,
            Summary = "No data available",
            Sequence = 0
        };

        await _state.WriteStateAsync();
    }

    public async Task ReceiveReminder(string reminderName, TickStatus status)
    {
        if(reminderName == "UpdateWeatherReminder")
        {
            Console.WriteLine($"Reminder triggered for {this.GetPrimaryKeyString()}: Updating weather data.");

            var forecast = _weatherService.Get().First();

            await _fusionCache.SetAsync(
                FormatCacheKey(this.GetPrimaryKeyString()),
                forecast
            );
        }
    }
}