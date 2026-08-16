using System.Threading.Channels;

namespace ResetService.Infrastructure.Commands;

public sealed class SequentialCommandQueue<TCommand>
{
    private readonly Channel<TCommand> _channel;

    public SequentialCommandQueue(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);

        _channel = Channel.CreateBounded<TCommand>(new BoundedChannelOptions(capacity)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait,
            AllowSynchronousContinuations = false,
        });
    }

    public ValueTask EnqueueAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        return _channel.Writer.WriteAsync(command, cancellationToken);
    }

    public ValueTask<TCommand> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAsync(cancellationToken);
    }

    public void StopAccepting()
    {
        _channel.Writer.TryComplete();
    }
}
