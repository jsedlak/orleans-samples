namespace SatellitePattern.Shared.Domain;

[GenerateSerializer]
public sealed class AccountState
{
    [Id(0)]
    public DirectMessage[] Messages { get; set; } = Array.Empty<DirectMessage>();
}