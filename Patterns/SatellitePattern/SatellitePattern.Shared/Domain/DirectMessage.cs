namespace SatellitePattern.Shared.Domain;

[GenerateSerializer]
public sealed class DirectMessage
{
    [Id(0)]
    public Guid MessageId { get; set; } = Guid.NewGuid();

    [Id(1)]
    public Guid SenderAccountId { get; set; }

    [Id(2)]
    public string Message { get; set; } = string.Empty;

    [Id(3)]
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    [Id(4)]
    public bool IsRead { get; set; } = false;
}
