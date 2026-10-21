using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitSharp.Buffer.Configurations;

namespace RabbitSharp.Buffer.BackgroundJobs;

public sealed class MessageQueueConnectionWorker(
    IOptions<RabbitMqConnectionConfiguration> options,
    MessageQueueConnectionBuilder messageQueueConnectionBuilder) : IHostedLifecycleService
{
    private IConnection? _connection;
    private IChannel? _channel;

    private readonly RabbitMqConnectionConfiguration _rabbitMqConnectionConfiguration =
        options.Value ?? throw new ArgumentException("RabbitMq Configuration Is Null or Empty");

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StartedAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StoppedAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async Task StartingAsync(CancellationToken cancellationToken)
    {
        try
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMqConnectionConfiguration.HostName,
                UserName = _rabbitMqConnectionConfiguration.UserName,
                Password = _rabbitMqConnectionConfiguration.Password,
                Port = int.Parse(_rabbitMqConnectionConfiguration.Port),
                VirtualHost = _rabbitMqConnectionConfiguration.VirtualHost
            };

            _connection = await connectionFactory.CreateConnectionAsync(cancellationToken: cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            foreach (ExchangeMetadata exchangeMetadata in _rabbitMqConnectionConfiguration.ExchangeMetadata)
            {
                await _channel.ExchangeDeclareAsync(
                    exchange: exchangeMetadata.ExchangeName,
                    type: exchangeMetadata.Type,
                    durable: true,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                foreach (Queue queue in exchangeMetadata.Queues)
                {
                    await _channel.QueueDeclareAsync(
                        queue: queue.QueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: new Dictionary<string, object?>()
                        {
                            {
                                "x-queue-type", "quorum"
                            }
                        },
                        cancellationToken: cancellationToken);

                    await _channel.QueueBindAsync(
                        queue: queue.QueueName,
                        exchange: exchangeMetadata.ExchangeName,
                        routingKey: queue.RoutingKey,
                        arguments: null,
                        noWait: true,
                        cancellationToken: cancellationToken);
                }
            }

            messageQueueConnectionBuilder.SetConnection(_connection);
            messageQueueConnectionBuilder.SetChannel(_channel);
        }
        catch (Exception exception)
        {
            messageQueueConnectionBuilder.SetException(exception);
        }
    }

    public async Task StoppingAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
        {
            await _channel.DisposeAsync();
        }

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
    }
}