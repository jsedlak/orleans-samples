namespace GzipSample.Shared.Actors;

public class BankAccountGrain : Grain<BalanceDetails>, IBankAccount
{
    public Task<BalanceDetails> CheckBalance()
    {
        return Task.FromResult(State);
    }

    public async Task Deposit(double amount)
    {
        State.TotalDeposits += amount;
        State.Amount += amount;

        await WriteStateAsync();
    }

    public async Task<double> Withdraw(double amount)
    {
        if(amount > State.Amount)
        {
            amount = State.Amount;
        }

        State.TotalWithdrawals += amount;
        State.Amount -= amount;

        await WriteStateAsync();

        return amount;
    }
}