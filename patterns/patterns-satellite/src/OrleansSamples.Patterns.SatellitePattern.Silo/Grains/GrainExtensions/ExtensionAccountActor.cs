using OrleansSamples.Patterns.SatellitePattern.Grains.GrainExtensions;

namespace OrleansSamples.Patterns.SatellitePattern.Silo.Grains.GrainExtensions;

public class ExtensionAccountActor : Grain, IExtensionAccountActor
{
    public async Task<bool> SignIn(string status)
    {
        var satelliteGrain = GrainFactory.GetGrain<IExtendedAccountStatusActor>(this.GetGrainId().GetGuidKey());
        var ext = satelliteGrain.AsReference<IAccountSatelliteActorExtension>();

        var result = await ext.SetStatus(status);

        return result;
    }

    public async Task<bool> SignOut()
    {
        var satelliteGrain = GrainFactory.GetGrain<IExtendedAccountStatusActor>(this.GetGrainId().GetGuidKey());
        var ext = satelliteGrain.AsReference<IAccountSatelliteActorExtension>();

        var result = await ext.SetStatus(null);

        return result;
    }
}
