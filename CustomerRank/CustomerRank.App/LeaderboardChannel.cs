using Domain;
using System.Threading.Channels;

namespace CustomerRank.App;

public class LeaderboardChannel
{
    private readonly Channel<Customer> _channel;
    private readonly CustomerSet _customerSet;
    private readonly Leaderboard _leaderboard;

    public LeaderboardChannel(CustomerSet customerSet,
            Leaderboard leaderboard)
    {
        BoundedChannelOptions options = new BoundedChannelOptions(10000);
        options.SingleWriter = false;
        options.SingleReader = true;
        options.AllowSynchronousContinuations = false;
        options.FullMode = BoundedChannelFullMode.Wait;
        _channel = Channel.CreateBounded<Customer>(options);

        _customerSet = customerSet;
        _leaderboard = leaderboard;
    }

    public async Task ReadAsync(CancellationToken cancellationToken)
    {
        while (await _channel.Reader.WaitToReadAsync())
        {
            while (_channel.Reader.TryRead(out var value))
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested) return;
                    Customer? customer = _customerSet.GetCustomer(value.CustomerId);
                    if (customer == null)
                    {
                        customer = new Customer();
                        customer.CustomerId = value.CustomerId;
                        customer.ScoreValue = value.ScoreValue;
                        _customerSet.AddCustomer(customer);
                    }
                    if (customer.IsZeroScore()) return;
                    _leaderboard.RemoveCustomer(customer);
                    customer.UpdateScore(value.ScoreValue);
                    Score score = _leaderboard.GetOneByScoreValue(customer.ScoreValue);
                    score.AddCustomer(customer);
                }
                catch (Exception ex)
                { 
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }
    }

    public void Write(Customer customer)
    { 
        _channel.Writer.TryWrite(customer);
    }
}
