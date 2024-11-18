namespace SatellitePattern.Shared.Domain;

[GenerateSerializer]
public sealed class OnlineStatus
{
    [Id(0)]
    public Guid AccountId { get; set; }

    [Id(1)]
    public string Status { get; set; } = string.Empty;

    [Id(2)]
    public bool IsOnline { get; set; }

    [Id(3)]
    public DateTime LastSeen { get; set; } = DateTime.UtcNow;

    public string ToDisplayString()
    {
        if (!IsOnline)
        {
            return "Offline";
        }

        return $"Last seen {LastSeen.ToShortTimeString()}, {Status}";
    }
}
