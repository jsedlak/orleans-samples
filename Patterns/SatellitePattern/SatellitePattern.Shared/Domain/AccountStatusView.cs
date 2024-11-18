using System.ComponentModel.DataAnnotations;

namespace SatellitePattern.Shared.Domain;

/// <summary>
/// Represents an event driven view model of a single account's status
/// </summary>
[GenerateSerializer]
public sealed class AccountStatusView
{
    [Id(0)]
    [Key]
    public Guid AccountId { get; set; }

    [Id(1)]
    public string Status { get; set; } = string.Empty;

    [Id(2)]
    public bool IsOnline { get; set; }
}