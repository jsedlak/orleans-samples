using Microsoft.AspNetCore.Mvc;
using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Grains.GrainExtensions;
using OrleansSamples.Patterns.SatellitePattern.Silo.ApiModel;

[ApiController]
[Route("api/[controller]")]
public class SatelliteExtensionController : ControllerBase
{
    private readonly IClusterClient _clusterClient;

    public SatelliteExtensionController(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }

    [HttpPost("{accountId}/signin")]
    public async Task SignIn([FromRoute]Guid accountId, [FromBody]SignInCommand command)
    {
        var account = _clusterClient.GetGrain<IExtensionAccountActor>(accountId);

        await account.SignIn(command.Status);
    }

    [HttpPost("{accountId}/signout")]
    public async Task SignOut([FromRoute] Guid accountId)
    {
        var account = _clusterClient.GetGrain<IExtensionAccountActor>(accountId);

        await account.SignOut();
    }

    [HttpGet("{accountId}")]
    public async Task<OnlineStatus> GetStatus([FromRoute] Guid accountId)
    {
        var account = _clusterClient.GetGrain<IExtendedAccountStatusActor>(accountId);

        return await account.GetStatus();
    }
}