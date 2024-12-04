namespace OrleansSamples.Patterns.SatellitePattern.Domain.Model;

/// <summary>
/// Stores the online status of an account
/// </summary>
[GenerateSerializer]
public sealed class OnlineStatus
{
    /// <summary>
    /// Gets or Sets the account id this status represents
    /// </summary>
    [Id(0)]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Gets or Sets the status of the account
    /// </summary>
    [Id(1)]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets whether the account is online
    /// </summary>
    [Id(2)]
    public bool IsOnline { get; set; }

    /// <summary>
    /// Gets or Sets the last time this status was updated
    /// </summary>
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
