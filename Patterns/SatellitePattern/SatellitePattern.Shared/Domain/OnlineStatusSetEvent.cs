namespace SatellitePattern.Shared.Domain;

[GenerateSerializer]
public class OnlineStatusSetEvent
{
    [Id(0)]
    public Guid AccountId { get; set; }

    [Id(1)]
    public string? Status { get; set; }
}
