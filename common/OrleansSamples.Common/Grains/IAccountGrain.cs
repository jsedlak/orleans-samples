using Orleans;
using OrleansSamples.Common.Model;

namespace OrleansSamples.Common.Grains;

/// <summary>
/// Represents a basic scenario of storing bank account information 
/// in a distributed system for a large number of users
/// </summary>
public interface IAccountGrain : IGrainWithIntegerKey 
{
    /// <summary>
    /// Deposits the specified amount of money into the account.
    /// </summary>
    /// <param name="amount">The amount to deposit.</param>
    /// <returns>The new account balance.</returns>
    ValueTask<double> Deposit(double amount);

    /// <summary>
    /// Attempts to withdraw an amount from the account. If the balance does 
    /// not cover the requested amount, the balance is returned instead.
    /// </summary>
    /// <param name="amount">The amount to withdraw</param>
    /// <returns>The amount withdrawn</returns>
    ValueTask<double> Withdraw(double amount);

    /// <summary>
    /// Gets the current balance
    /// </summary>
    /// <returns>The amount of money in the account.</returns>
    Task<AccountBalance> GetBalance();
}
