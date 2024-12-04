using Microsoft.AspNetCore.Mvc;
using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Grains.BasicSatelliteGrain;
using OrleansSamples.Patterns.SatellitePattern.Silo.ApiModel;

[ApiController]
[Route("api/[controller]")]
public class BasicController : ControllerBase
{
    private readonly IClusterClient _clusterClient;

    public BasicController(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }

    [HttpPost("{accountId}/signin")]
    public async Task SignIn([FromRoute]Guid accountId, [FromBody]SignInCommand command)
    {
        var account = _clusterClient.GetGrain<IBasicAccountActor>(accountId);

        await account.SignIn(command.Status);
    }

    [HttpPost("{accountId}/signout")]
    public async Task SignOut([FromRoute] Guid accountId)
    {
        var account = _clusterClient.GetGrain<IBasicAccountActor>(accountId);

        await account.SignOut();
    }

    [HttpGet("{accountId}")]
    public async Task<OnlineStatus> GetStatus([FromRoute] Guid accountId)
    {
        var account = _clusterClient.GetGrain<IAccountStatusActor>(accountId);

        return await account.GetStatus();
    }
}