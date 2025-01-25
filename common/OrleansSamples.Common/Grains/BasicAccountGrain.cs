using OrleansSamples.Common.Model;

namespace OrleansSamples.Common.Grains;

public sealed class BasicAccountGrain : Grain, IAccountGrain 
{
    private readonly IPersistentState<AccountBalance> _accountBalance;

    public BasicAccountGrain([PersistentState("accountBalance")] IPersistentState<AccountBalance> state)
    {
        _accountBalance = state;
    }

    public async ValueTask<double> Deposit(double amount)
    {
        _accountBalance.State.Amount += amount;
        
        await _accountBalance.WriteStateAsync();

        return _accountBalance.State.Amount;
    }

    public async ValueTask<double> Withdraw(double amount)
    {
        amount = Math.Min(amount, _accountBalance.State.Amount);
        _accountBalance.State.Amount -= amount;

        await _accountBalance.WriteStateAsync();

        return amount;
    }

    public Task<AccountBalance> GetBalance()
    {
        return Task.FromResult(_accountBalance.State);
    }
}