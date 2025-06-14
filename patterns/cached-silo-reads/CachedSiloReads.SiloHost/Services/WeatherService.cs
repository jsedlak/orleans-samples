using CachedSiloReads.SiloHost.Model;

namespace CachedSiloReads.SiloHost.Services;

public class WeatherService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private int SequenceCounter = 0;

    public IEnumerable<WeatherForecast> Get()
    {
        var sequence = ++SequenceCounter;

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            Sequence = sequence
        })
        .ToArray();
    }
}
