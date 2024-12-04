namespace OrleansSamples.Common.Model;

[GenerateSerializer]
public class AccountBalance
{
    [Id(0)]
    public double Amount { get; set; }
}