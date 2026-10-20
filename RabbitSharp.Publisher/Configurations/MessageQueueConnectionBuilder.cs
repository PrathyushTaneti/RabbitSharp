using RabbitMQ.Client;

namespace RabbitSharp.Publisher.Configurations;

/*
    Update your class to use TaskCompletionSource. This allows the hosted service to "signal"
    when the connection and channel are ready, while keeping your properties as Task objects
    that other services can await
*/
public sealed class MessageQueueConnectionBuilder
{
    private readonly TaskCompletionSource<IConnection> _connectionCompletionSource = new();
    private readonly TaskCompletionSource<IChannel> _channelCompletionSource = new();

    public Task<IConnection> Connection => _connectionCompletionSource.Task;
    public Task<IChannel> Channel => _channelCompletionSource.Task;

    internal void SetConnection(IConnection connection) => _connectionCompletionSource.TrySetResult(connection);
    internal void SetChannel(IChannel channel) => _channelCompletionSource.TrySetResult(channel);

    internal void SetException(Exception exception)
    {
        _connectionCompletionSource.SetException(exception);
        _channelCompletionSource.SetException(exception);
    }
}