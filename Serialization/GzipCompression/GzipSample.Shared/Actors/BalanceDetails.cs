namespace GzipSample.Shared.Actors;

[GenerateSerializer]
public sealed class BalanceDetails
{
    [Id(0)]
    public double Amount { get; set; }

    [Id(1)]
    public double TotalWithdrawals { get; set; }

    [Id(2)]
    public double TotalDeposits { get; set; }
}
