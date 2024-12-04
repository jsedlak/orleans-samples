namespace OrleansSamples.Patterns.SatellitePattern.Domain.Aggregates;

public interface IAccount
{
    Task<bool> SignIn(string status);

    Task<bool> SignOut();
}
