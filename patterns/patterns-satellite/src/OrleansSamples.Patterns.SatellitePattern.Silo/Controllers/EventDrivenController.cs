using Microsoft.AspNetCore.Mvc;
using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Grains.EventDriven;
using OrleansSamples.Patterns.SatellitePattern.Silo.ApiModel;

[ApiController]
[Route("api/[controller]")]
public class EventDrivenController : ControllerBase
{
    private readonly IClusterClient _clusterClient;

    public EventDrivenController(IClusterClient clusterClient)
    {
        _clusterClient = clusterClient;
    }

    [HttpPost("{accountId}/signin")]
    public async Task SignIn([FromRoute]Guid accountId, [FromBody]SignInCommand command)
    {
        var account = _clusterClient.GetGrain<IEventDrivenAccountActor>(accountId);

        await account.SignIn(command.Status);
    }

    [HttpPost("{accountId}/signout")]
    public async Task SignOut([FromRoute] Guid accountId)
    {
        var account = _clusterClient.GetGrain<IEventDrivenAccountActor>(accountId);

        await account.SignOut();
    }

    [HttpGet("{accountId}")]
    public async Task<OnlineStatus> GetStatus([FromRoute] Guid accountId)
    {
        var account = _clusterClient.GetGrain<IAccountStatusReporterActor>(0);

        return await account.GetStatus(accountId);
    }

    [HttpGet()]
    public async Task<IEnumerable<OnlineStatus>> GetStatuses()
    {
        var account = _clusterClient.GetGrain<IAccountStatusReporterActor>(0);

        return await account.GetStatuses();
    }

    [HttpGet("count")]
    public async Task<int> GetOnlineCount()
    {
        var account = _clusterClient.GetGrain<IAccountStatusReporterActor>(0);

        return await account.GetOnlineCount();
    }
}