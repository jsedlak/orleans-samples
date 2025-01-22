
using Microsoft.Extensions.Logging;
using OrleansSamples.Common.Model;

namespace OrleansSamples.Common.Grains;

public class ReminderCounterGrain : Grain, IReminderCounterGrain, IRemindable
{
    private const string ReminderName = "Counter";

    private readonly IPersistentState<Count> _count;
    private readonly ILogger<IReminderCounterGrain> _logger;

    public ReminderCounterGrain(
        [PersistentState("count")] IPersistentState<Count> count,
        ILogger<IReminderCounterGrain> logger)
    {
        _count = count;
        _logger = logger;
    }

    public async Task ReceiveReminder(string reminderName, TickStatus status)
    {
        if(reminderName is ReminderName)
        {
            _count.State.Current++;
            await _count.WriteStateAsync();
            _logger.LogInformation($"[{this.GetGrainId().ToString()}] Count is now {_count.State.Current}");
        }
    }

    public async Task<Count> StartCounting()
    {
        // Minimum reminder interval is 60 seconds
        await this.RegisterOrUpdateReminder(ReminderName, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(60));
        return _count.State;
    }

    public async Task<Count> StopCounting()
    {
        var reminder = await this.GetReminder(ReminderName);
        await this.UnregisterReminder(reminder);

        return _count.State;
    }

    public Task<Count> GetCount()
    {
        return Task.FromResult(_count.State);
    }
}