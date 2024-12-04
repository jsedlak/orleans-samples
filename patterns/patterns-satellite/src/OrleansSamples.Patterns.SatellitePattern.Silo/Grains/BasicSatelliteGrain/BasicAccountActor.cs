using OrleansSamples.Patterns.SatellitePattern.Grains.BasicSatelliteGrain;

namespace OrleansSamples.Patterns.SatellitePattern.Silo.Grains.BasicSatelliteGrain;

public class BasicAccountActor : Grain, IBasicAccountActor
{
    public async Task<bool> SignIn(string status)
    {
        await GrainFactory
            .GetGrain<IAccountStatusActor>(this.GetGrainId().GetGuidKey())
            .SetStatus(status);

        return true;
    }

    public async Task<bool> SignOut()
    {
        await GrainFactory
            .GetGrain<IAccountStatusActor>(this.GetGrainId().GetGuidKey())
            .SetStatus(null);

        return true;
    }
}
