using OrleansSamples.Common.Model;

namespace OrleansSamples.Common.Grains;

public sealed class BasicAccountGrain : Grain<AccountBalance>, IAccountGrain 
{
    public async ValueTask<double> Deposit(double amount)
    {
        State.Amount += amount;
        
        await WriteStateAsync();

        return State.Amount;
    }

    public async ValueTask<double> Withdraw(double amount)
    {
        amount = Math.Min(amount, State.Amount);
        State.Amount -= amount;

        await WriteStateAsync();

        return amount;
    }

    public Task<AccountBalance> GetBalance()
    {
        return Task.FromResult(State);
    }
}