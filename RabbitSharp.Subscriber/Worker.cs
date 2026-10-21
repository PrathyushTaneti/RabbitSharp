using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitSharp.Buffer.Configurations;

namespace RabbitSharp.Subscriber;

public class Worker(ILogger<Worker> logger, MessageQueueConnectionBuilder messageQueueConnectionBuilder)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        IChannel channel = await messageQueueConnectionBuilder.Channel;
        using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(40));
        while (stoppingToken.IsCancellationRequested is false &&
               await periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            AsyncEventingBasicConsumer consumer = new(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                logger.LogInformation("Data Logged : {data}", message);
                return Task.CompletedTask;
            };
            await channel.BasicConsumeAsync("bills",
                autoAck: true,
                consumer: consumer,
                cancellationToken: stoppingToken);
        }
    }
}
