namespace OrleansSamples.Patterns.SatellitePattern.Domain.Events;

[GenerateSerializer]
public sealed class OnlineStatusSetEvent
{
    [Id(0)]
    public Guid AccountId { get; set; }

    [Id(1)]
    public string? Status { get; set; }
}
