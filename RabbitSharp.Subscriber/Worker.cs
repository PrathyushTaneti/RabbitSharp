using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitSharp.Subscriber.Configurations;

namespace RabbitSharp.Subscriber;

public class Worker(ILogger<Worker> logger, IOptions<RabbitMQConnectionFactory> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        while (stoppingToken.IsCancellationRequested is false &&
               await periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            CancellationTokenSource cancellationTokenSource = new();
            cancellationTokenSource.CancelAfter(delay: TimeSpan.FromMinutes(minutes: 5));

            ConnectionFactory connectionFactory = new()
            {
                HostName = options.Value.HostName,
                UserName = options.Value.UserName,
                Password = options.Value.Password,
                Port = int.Parse(options.Value.Port),
                VirtualHost = options.Value.VirtualHost
            };

            await using IConnection connection =
                await connectionFactory.CreateConnectionAsync(cancellationToken: cancellationTokenSource.Token);

            await using IChannel channel =
                await connection.CreateChannelAsync(cancellationToken: cancellationTokenSource.Token);

            await channel.QueueDeclareAsync(
                queue: "samplequeue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object?>()
                {
                    {
                        "x-queue-type", "quorum"
                    }
                },
                cancellationToken: cancellationTokenSource.Token);


            logger.LogInformation("[*] Waiting for messages");

            AsyncEventingBasicConsumer consumer = new(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Worker running at: {time} and received data : {message}",
                        DateTime.UtcNow.ToString(),
                        message);
                }

                return Task.CompletedTask;
            };
        }
    }
}
