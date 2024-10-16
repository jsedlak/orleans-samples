namespace GzipSample.Shared.Actors;

public interface IBankAccount : IGrainWithIntegerKey
{
    Task Deposit(double amount);

    Task<double> Withdraw(double amount);

    Task<BalanceDetails> CheckBalance();
}
