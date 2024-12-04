using Microsoft.AspNetCore.Mvc;
using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Domain.ServiceModel;
using OrleansSamples.Patterns.SatellitePattern.Grains.EventDrivenDatabase;
using OrleansSamples.Patterns.SatellitePattern.Silo.ApiModel;

[ApiController]
[Route("api/[controller]")]
public class EventDrivenDatabaseController : ControllerBase
{
    private readonly IClusterClient _clusterClient;
    private readonly IAccountStatusReadRepository _accountStatusReadRepository;

    public EventDrivenDatabaseController(IClusterClient clusterClient, IAccountStatusReadRepository accountStatusReadRepository)
    {
        _clusterClient = clusterClient;
        _accountStatusReadRepository = accountStatusReadRepository;
    }

    [HttpPost("{accountId}/signin")]
    public async Task SignIn([FromRoute]Guid accountId, [FromBody]SignInCommand command)
    {
        var account = _clusterClient.GetGrain<IEventDrivenDatabaseAccountActor>(accountId);

        await account.SignIn(command.Status);
    }

    [HttpPost("{accountId}/signout")]
    public async Task SignOut([FromRoute] Guid accountId)
    {
        var account = _clusterClient.GetGrain<IEventDrivenDatabaseAccountActor>(accountId);

        await account.SignOut();
    }

    [HttpGet("{accountId}")]
    public async Task<OnlineStatus> GetStatus([FromRoute] Guid accountId)
    {
        return await _accountStatusReadRepository.GetStatus(accountId) ?? new();
    }

    [HttpGet()]
    public async Task<IEnumerable<OnlineStatus>> GetStatuses()
    {
        return await _accountStatusReadRepository.GetStatuses();
    }

    [HttpGet("count")]
    public async Task<int> GetOnlineCount()
    {
        return await _accountStatusReadRepository.GetOnlineCount();
    }
}