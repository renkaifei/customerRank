
using CustomerRank.App;
using Domain;

namespace CustomerRank.HostedServices;

public class LeaderboardHostedService : IHostedService
{
    private readonly LeaderboardChannel _channel;

    public LeaderboardHostedService(LeaderboardChannel channel)
    {
        _channel = channel;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(async () =>
        {
            await _channel.ReadAsync(cancellationToken);
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
